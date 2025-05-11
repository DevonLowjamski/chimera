import json
import os
from pathlib import Path
from unittest.mock import MagicMock, patch  # Only import patch and MagicMock

import pytest
from pydantic_core import ValidationError  # Import Pydantic's specific error

# Assuming your config module is in src/config
# Adjust the import path based on your project structure and how tests are run
from src.config.schema import DEFAULT_CONFIG_FILE, AppConfig, save_config


# Fixture for managing environment variables
@pytest.fixture(autouse=True)
def manage_environment(monkeypatch):
    env_backup = os.environ.copy()
    yield monkeypatch
    os.environ.clear()
    os.environ.update(env_backup)


# Fixture for temporary config file
@pytest.fixture
def temp_config_file(tmp_path):
    config_dir = tmp_path / "config_test"
    config_dir.mkdir()
    config_file = config_dir / "test_config.json"
    return config_file


# Fixture for temporary valid directory
@pytest.fixture
def temp_valid_dir(tmp_path):
    valid_dir = tmp_path / "valid_research_dir"
    valid_dir.mkdir()
    # Ensure it's writable for the test
    os.chmod(valid_dir, 0o777)
    return valid_dir


# === Test AppConfig Loading ===


def test_default_config_loading(temp_valid_dir):
    """Test loading config with no file and no env vars, using defaults."""
    # Ensure default config file doesn't exist
    if DEFAULT_CONFIG_FILE.exists():
        os.remove(DEFAULT_CONFIG_FILE)

    # Ensure data_dir exists for default check
    default_data_dir = Path("./data")
    default_data_dir.mkdir(exist_ok=True)

    # Mock os.access to simulate permissions for default data_dir if needed
    with patch("os.access", return_value=True):
        # Load config - should use defaults and derive research_folder from data_dir
        cfg = AppConfig.load(config_file=Path("/non/existent/path.json"))

    assert cfg.app_name == "ResearchContextMCP"
    assert cfg.debug_mode is False
    assert cfg.log_level == "INFO"
    assert cfg.data_dir == default_data_dir
    # Check if research_folder_path defaulted to data_dir
    assert cfg.research_folder_path == default_data_dir
    # Clean up created data dir if necessary
    if default_data_dir.exists() and not any(default_data_dir.iterdir()):
        # Only remove if empty and created by this test setup
        # Be cautious with rmdir in tests
        pass  # Or os.rmdir(default_data_dir) if safe


def test_config_loading_from_file(temp_config_file, temp_valid_dir):
    """Test loading configuration primarily from a JSON file."""
    file_data = {
        "app_name": "FileAppName",
        "debug_mode": True,
        "log_level": "DEBUG",
        "research_folder_path": str(temp_valid_dir),
    }
    with open(temp_config_file, "w") as f:
        json.dump(file_data, f)

    # Mock os.access to ensure path validation passes
    with patch("os.access", return_value=True):
        cfg = AppConfig.load(config_file=temp_config_file)

    assert cfg.app_name == "FileAppName"
    assert cfg.debug_mode is True
    assert cfg.log_level == "DEBUG"
    assert cfg.research_folder_path == temp_valid_dir
    assert cfg.data_dir == Path("./data")  # Should still use default if not in file/env


def test_env_variable_override(temp_config_file, temp_valid_dir, manage_environment):
    """Test that environment variables override file settings and defaults."""
    file_data = {
        "app_name": "FileAppName",
        "debug_mode": False,
        "research_folder_path": "/file/path",  # Intentionally different
    }
    with open(temp_config_file, "w") as f:
        json.dump(file_data, f)

    # Set environment variables
    manage_environment.setenv("APP_NAME", "EnvAppName")
    manage_environment.setenv("DEBUG_MODE", "1")  # Test different truthy value
    manage_environment.setenv("LOG_LEVEL", "WARNING")
    manage_environment.setenv("RESEARCH_FOLDER_PATH", str(temp_valid_dir))
    manage_environment.setenv("DATA_DIR", str(temp_valid_dir / "env_data"))

    # Mock os.access for path validation
    with patch("os.path.isdir", return_value=True), patch(
        "os.access", return_value=True
    ):
        cfg = AppConfig.load(config_file=temp_config_file)

    assert cfg.app_name == "EnvAppName"
    assert cfg.debug_mode is True
    assert cfg.log_level == "WARNING"
    assert cfg.research_folder_path == temp_valid_dir
    assert cfg.data_dir == temp_valid_dir / "env_data"
    # Check that the env data dir was created by the validator
    assert (temp_valid_dir / "env_data").is_dir()


def test_invalid_json_file(temp_config_file, temp_valid_dir):
    """Test loading with an invalid JSON file, should use defaults/env."""
    # Test now checks the resulting config values instead of capturing logs
    with open(temp_config_file, "w") as f:
        f.write("this is not json{")  # Invalid JSON

    default_data_dir = Path("./data")
    default_data_dir.mkdir(exist_ok=True)

    with patch("os.access", return_value=True):  # Mock access for default data_dir
        cfg = AppConfig.load(config_file=temp_config_file)

    # Assert that config fell back to defaults because file was invalid
    assert cfg.app_name == "ResearchContextMCP"
    assert cfg.debug_mode is False
    assert cfg.research_folder_path == default_data_dir  # Should default to data_dir


def test_nonexistent_config_file(temp_valid_dir):
    """Test specifying a config file that doesn't exist, should use defaults/env."""
    # Test now checks the resulting config values instead of capturing logs
    non_existent_file = Path("/path/to/non/existent/config.json")
    assert not non_existent_file.exists()

    default_data_dir = Path("./data")
    default_data_dir.mkdir(exist_ok=True)

    with patch("os.access", return_value=True):  # Mock access for default data_dir
        cfg = AppConfig.load(config_file=non_existent_file)

    # Assert that config used defaults because specified file wasn't found
    assert cfg.app_name == "ResearchContextMCP"
    assert cfg.debug_mode is False
    assert cfg.research_folder_path == default_data_dir  # Should default to data_dir


# === Test research_folder_path Validation ===


def test_research_path_validation_valid(temp_valid_dir):
    """Test validation with a valid, existing, accessible directory."""
    data = {"research_folder_path": str(temp_valid_dir)}
    with patch("os.access", return_value=True):  # Mock access check
        config = AppConfig(**data)
    assert config.research_folder_path == temp_valid_dir


def test_research_path_validation_nonexistent(tmp_path):
    """Test validation with a path that does not exist."""
    non_existent_path = tmp_path / "nonexistent"
    data = {"research_folder_path": str(non_existent_path)}
    # Match core message within the Pydantic error
    with pytest.raises(
        ValidationError, match=r"Provided research dir does not exist:.*"
    ):
        AppConfig(**data)


def test_research_path_validation_not_a_dir(tmp_path):
    """Test validation with a path that exists but is a file."""
    file_path = tmp_path / "a_file.txt"
    file_path.touch()
    data = {"research_folder_path": str(file_path)}
    # Match core message
    with pytest.raises(
        ValidationError, match=r"Provided research path is not a dir:.*"
    ):
        AppConfig(**data)


def test_research_path_validation_not_readable(temp_valid_dir):
    """Test validation with a directory that is not readable."""
    data = {"research_folder_path": str(temp_valid_dir)}
    with patch("os.access", side_effect=lambda path, mode: mode != os.R_OK):
        # Match core message
        with pytest.raises(
            ValidationError, match=r"Provided research dir not readable:.*"
        ):
            AppConfig(**data)


def test_research_path_validation_not_writable(temp_valid_dir):
    """Test validation with a directory that is not writable."""
    data = {"research_folder_path": str(temp_valid_dir)}
    with patch("os.access", side_effect=lambda path, mode: mode != os.W_OK):
        # Match core message
        with pytest.raises(
            ValidationError, match=r"Provided research dir not writable:.*"
        ):
            AppConfig(**data)


def test_research_path_validation_none_no_default():
    """Test validation when path is None and data_dir isn't valid/set."""
    invalid_data_dir_path = "./non_existent_or_inaccessible_data_dir"
    with patch("os.access", return_value=False):
        # Match core message from the model_validator
        with pytest.raises(
            ValidationError,
            match=r"research_folder_path is required and could not be defaulted.*",
        ):
            AppConfig(data_dir=invalid_data_dir_path, research_folder_path=None)


# === Test data_dir Validation ===


def test_data_dir_creation(tmp_path):
    """Test that the data_dir is created if it doesn't exist."""
    data_dir_path = tmp_path / "test_data_dir"
    assert not data_dir_path.exists()
    config = AppConfig(data_dir=str(data_dir_path))
    assert data_dir_path.exists()
    assert data_dir_path.is_dir()
    assert config.data_dir == data_dir_path


# === Updated and New Tests for save_config Function ===


def test_save_config_creates_file(temp_config_file, temp_valid_dir):
    """Test saving creates the file with correct content."""
    config_to_save = AppConfig(
        research_folder_path=str(temp_valid_dir), data_dir=temp_valid_dir / "data"
    )
    save_config(config_to_save, config_file=temp_config_file)

    assert temp_config_file.exists()
    with open(temp_config_file, "r") as f:
        saved_data = json.load(f)
    assert saved_data["app_name"] == "ResearchContextMCP"
    assert saved_data["research_folder_path"] == str(temp_valid_dir)
    assert "config_file_path" not in saved_data


def test_save_config_creates_backup(temp_config_file, temp_valid_dir):
    """Test saving creates a backup (.bak) when overwriting."""
    backup_path = temp_config_file.with_suffix(".json.bak")
    # Create an initial file to be backed up
    initial_content = {"app_name": "Initial", "debug_mode": True}
    with open(temp_config_file, "w") as f:
        json.dump(initial_content, f)

    assert temp_config_file.exists()
    assert not backup_path.exists()

    # Save new config
    config_to_save = AppConfig(
        research_folder_path=str(temp_valid_dir), app_name="Updated"
    )
    save_config(config_to_save, config_file=temp_config_file)

    # Check backup exists and has initial content
    assert backup_path.exists()
    with open(backup_path, "r") as f:
        backup_data = json.load(f)
    assert backup_data == initial_content

    # Check original file has new content
    with open(temp_config_file, "r") as f:
        saved_data = json.load(f)
    assert saved_data["app_name"] == "Updated"


def test_save_config_no_backup_on_first_save(temp_config_file, temp_valid_dir):
    """Test backup is not created if the config file doesn't exist initially."""
    backup_path = temp_config_file.with_suffix(".json.bak")
    assert not temp_config_file.exists()
    assert not backup_path.exists()

    config_to_save = AppConfig(research_folder_path=str(temp_valid_dir))
    save_config(config_to_save, config_file=temp_config_file)

    assert temp_config_file.exists()
    assert not backup_path.exists()  # No backup should be created


@patch("src.config.schema.os.replace")
@patch("src.config.schema.tempfile.NamedTemporaryFile")
@patch("src.config.schema.os.fsync")
@patch("src.config.schema.os.path.exists")
@patch("src.config.schema.os.unlink")
def test_save_config_atomic_failure_cleanup(
    mock_unlink,
    mock_os_path_exists,
    mock_os_fsync,
    mock_named_temp_file,
    mock_os_replace,
    temp_config_file,
    temp_valid_dir,
):
    """Test atomic write failure: tmp file is cleaned up, original untouched."""
    # Mock NamedTemporaryFile
    mock_temp_file_obj = MagicMock()
    tmp_file_name = str(temp_config_file.parent / (temp_config_file.name + ".tmp123"))
    mock_temp_file_obj.name = tmp_file_name
    mock_temp_file_cm = MagicMock()
    mock_temp_file_cm.__enter__.return_value = mock_temp_file_obj
    mock_named_temp_file.return_value = mock_temp_file_cm

    # Mock os.replace to fail
    mock_os_replace.side_effect = OSError("Atomic replace failed for test")

    # Mock os.path.exists: return True for the temp file path, False otherwise (or default)
    # Use a side_effect function for more control
    def exists_side_effect(path):
        if path == tmp_file_name:
            return True
        return os.path.exists(
            path
        )  # Call original for other paths if needed, but maybe just False

    # For simplicity in this test, let's assume only the temp file existence matters here
    mock_os_path_exists.side_effect = lambda path: path == tmp_file_name

    # Create initial file
    initial_content = {"app_name": "Initial", "debug_mode": False}
    with open(temp_config_file, "w") as f:
        json.dump(initial_content, f)
    initial_stat = temp_config_file.stat()

    config_to_save = AppConfig(
        research_folder_path=str(temp_valid_dir), app_name="AttemptedUpdate"
    )

    # Run save_config and expect RuntimeError
    with pytest.raises(RuntimeError, match="Failed to save configuration"):
        save_config(config_to_save, config_file=temp_config_file)

    # Assertions
    mock_named_temp_file.assert_called_once()
    mock_temp_file_obj.flush.assert_called_once()
    mock_os_fsync.assert_called_once_with(mock_temp_file_obj.fileno())
    mock_os_replace.assert_called_once_with(mock_temp_file_obj.name, temp_config_file)
    # Check that the exists check was performed within the except block
    mock_os_path_exists.assert_called_with(tmp_file_name)
    mock_unlink.assert_called_once_with(tmp_file_name)  # Should now be called

    # Check original file untouched
    assert temp_config_file.exists()
    with open(temp_config_file, "r") as f:
        final_content = json.load(f)
    assert final_content == initial_content
    assert temp_config_file.stat().st_mtime == initial_stat.st_mtime
