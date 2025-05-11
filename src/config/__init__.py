from .interface import get_config, set_research_folder_path
from .manager import ConfigurationManager
from .schema import DEFAULT_CONFIG_DIR, DEFAULT_CONFIG_FILE, AppConfig, save_config

# --- Global config instance (uses imported AppConfig and DEFAULT_CONFIG_FILE) ---
# This was a placeholder from an earlier stage and should likely be removed or handled by the ConfigurationManager
# config = AppConfig.load()

__all__ = [
    "AppConfig",
    "ConfigurationManager",
    "DEFAULT_CONFIG_DIR",
    "DEFAULT_CONFIG_FILE",
    "save_config",
    "get_config",
    "set_research_folder_path",
]
