# Research Context MCP

The Research Context MCP (My Context Provider) enhances AI coding agents by providing them with relevant context extracted from a user-specified collection of research reports. This project aims to extend capabilities similar to how Context7 understands programming documents to research-oriented documents. This enables AI agents to leverage detailed research findings, methodologies, and data to generate more accurate, precise, and contextually relevant code, answers, and analyses.

## Project Status

Alpha. Initial structure and basic configuration management in progress.

## Project Structure

```
/Users/devon/Documents/Cursor/Projects/
├── .github/                    # GitHub Actions workflows
│   └── workflows/
│       └── python-tests.yml    # CI workflow for Python tests
├── .vscode/                    # VSCode specific settings (gitignored)
├── data/                       # Directory for application data (e.g., indexed reports)
│   └── .gitkeep                # Ensures the data directory is tracked by git
├── docs/                       # Project documentation
├── scripts/                    # Utility and setup scripts
│   ├── example_prd.txt       # Example PRD structure
│   ├── prd.txt               # This project's PRD
│   └── setup_dev.sh          # Script to set up the development environment
├── src/                        # Source code for the application
│   ├── config/                 # Configuration management module
│   │   └── __init__.py         # Contains AppConfig and default config instance
│   ├── models/                 # Data models (e.g., for documents, index entries)
│   │   └── __init__.py
│   ├── services/               # Business logic and core services
│   │   └── __init__.py
│   ├── utils/                  # Utility functions and classes
│   │   └── __init__.py
│   ├── __init__.py
│   └── main.py                 # Main application entry point
├── tasks/                      # TaskMaster generated task files (JSON, Markdown)
│   └── tasks.json              # Main tasks file
├── tests/                      # Unit and integration tests
│   └── __init__.py
├── venv/                       # Python virtual environment (gitignored)
├── .gitignore                  # Specifies intentionally untracked files
├── .pre-commit-config.yaml     # Configuration for pre-commit hooks
├── CHANGELOG.md                # Log of notable changes to the project
├── pyproject.toml              # Python project configuration (for build tools, linters)
├── README.md                   # This file
├── requirements.txt            # Core application dependencies
└── setup.py                    # Setuptools script for packaging and installation
```

## Setup and Installation

1.  **Clone the repository (if applicable):**
    ```bash
    git clone <repository-url>
    cd ResearchContextMCP
    ```

2.  **Set up the development environment:**
    This script will create a virtual environment, install dependencies, and set up pre-commit hooks.
    ```bash
    chmod +x scripts/setup_dev.sh
    ./scripts/setup_dev.sh
    ```

3.  **Activate the virtual environment:**
    ```bash
    source venv/bin/activate
    ```
    (On Windows: `venv\Scripts\activate`)

4.  **Set Environment Variables (Optional/As Needed):**
    Create a `.env` file in the project root if you need to override default configurations (e.g., `APP_NAME`, `DEBUG_MODE`, `LOG_LEVEL`, `DATA_DIR`).
    Example `.env` file:
    ```env
    APP_NAME="My Research App"
    DEBUG_MODE=True
    LOG_LEVEL="DEBUG"
    DATA_DIR="./my_research_data"
    ```
    The application will load these variables if a `python-dotenv` package is used and integrated into the config logic.

## Usage

(To be documented as features are developed)

*   Configuring the research folder path.
*   How AI agents interact with the MCP.

## Running Tests

To run the test suite:
```bash
source venv/bin/activate
pytest
```

To run tests with coverage:
```bash
pytest --cov=src
```

## Contributing

(To be documented - e.g., coding standards, pull request process)
