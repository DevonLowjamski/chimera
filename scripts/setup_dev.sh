#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

# Create and activate virtual environment if it doesn't exist
if [ ! -d "venv" ]; then
  echo "Creating virtual environment..."
  python3 -m venv venv
fi

source venv/bin/activate

echo "Installing/updating dependencies..."

# Install core dependencies
pip install -r requirements.txt

# Install development dependencies from setup.py
# The [dev] extra needs to be defined in setup.py's extras_require
# Ensure setup.py has: extras_require={"dev": ["pytest", "pytest-cov", "black", "isort", "mypy"]}
pip install -e .[dev]

# Setup pre-commit hooks if .pre-commit-config.yaml exists
if [ -f ".pre-commit-config.yaml" ]; then
  echo "Setting up pre-commit hooks..."
  pre-commit install
fi

echo "Development environment setup complete."
echo "Activate the virtual environment by running: source venv/bin/activate"
