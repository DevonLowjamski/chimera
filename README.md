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

### Command Line Interface (CLI)

Once the package is installed (e.g., via `pip install .` or `pip install -e .` for development), you can use the `research-mcp` (or the alias `rmcp`) command line tool to manage the application's configuration.

**Available Commands:**

*   `research-mcp get`
    *   Displays the current application configuration, including the research folder path, app name, debug mode, etc.
*   `research-mcp set-folder <path_to_folder>`
    *   Sets the path to the research folder where your documents are stored. The application will try to create the folder if it doesn't exist.
    *   Example: `research-mcp set-folder ~/Documents/MyResearchPapers`
*   `research-mcp version`
    *   Shows the installed version of the Research Context MCP application.
*   `research-mcp location`
    *   Displays the file system path where the configuration file is stored.

**Example:**

```bash
# Activate your virtual environment first
# source venv/bin/activate

# Get current configuration
research-mcp get

# Set the research folder
research-mcp set-folder ./my_research_files

# Check the version
research-mcp version

# See where the config file is stored
research-mcp location
```

## Running Tests

To run the test suite:
```

## Embedding Generation System

The project uses a local embedding system based on [Sentence-Transformers](https://www.sbert.net/), enabling semantic search and vector-based document retrieval.

### Usage
- The `EmbeddingService` class in `src/services/embedding_service.py` loads a Sentence-Transformers model and generates embeddings for text chunks.
- Embeddings are generated automatically as part of the document processing pipeline (`DocumentParser`).
- Each `TextChunk` object will have an `embedding` field (a list of floats) after processing.

### Configuration
- **Model selection:** Set the model name via the `EMBEDDING_MODEL` environment variable or pass it to `EmbeddingService(model_name=...)`.
- **Device selection:** By default, uses CUDA if available; override with `device` argument or `CUDA_VISIBLE_DEVICES` env var.
- **Batching:** EmbeddingService batches input for efficiency; batch size can be configured in the constructor.

### Testing
- Run `pytest tests/services/test_embedding_service.py` to verify embedding generation and pipeline integration.
- Tests check for embedding presence, shape, and consistency for identical input.

### Extension Points
- Swap out the model by changing the model name in config or environment.
- Extend `EmbeddingService` to add caching, remote API support, or additional error handling as needed.

## Vector Index for Embedding-Based Retrieval

The `InMemoryVectorIndex` provides fast, in-memory access to all embedding vectors and their associated text chunks, enabling efficient semantic search and similarity queries. It is designed for extensibility to persistent or distributed vector stores.

### Quickstart Usage
```python
from src.services.vector_index import InMemoryVectorIndex
from src.chunking.text_chunk import TextChunk

# Initialize the index
index = InMemoryVectorIndex(embedding_dim=384)

# Add a chunk
chunk = TextChunk(content="example", source_document_id="doc1")
index.add([0.1]*384, chunk)

# Search for similar vectors
results = index.search([0.1]*384, top_k=5)
for r in results:
    print(r['score'], r['chunk_id'], r['content'])

# Remove a chunk
index.remove(chunk.chunk_id)
```

### API Reference
- `__init__(embedding_dim: int)`
    - Initialize the index with the given embedding dimension.
- `add(embedding: List[float], chunk_ref: Union[TextChunk, str]) -> None`
    - Add an embedding and its reference. Raises `ValueError` on error.
- `get_embedding(chunk_id: str) -> Optional[np.ndarray]`
    - Retrieve the embedding vector for a chunk ID.
- `get_chunk_ref(chunk_id: str) -> Optional[TextChunk | str]`
    - Retrieve the chunk reference for a chunk ID.
- `remove(chunk_id: str) -> bool`
    - Remove an embedding and its reference by chunk ID.
- `search(query: List[float], top_k: int = 5) -> List[dict]`
    - Search for the top_k most similar embeddings using cosine similarity. Returns a list of dicts with `score`, `chunk_id`, `content`, and `metadata`.
- `__len__()`
    - Return the number of vectors in the index.

### Extension Guide
- **Custom Storage:** Subclass `InMemoryVectorIndex` and override storage logic for persistent or distributed backends.
- **Custom Similarity:** Override the `search` method to implement a different similarity metric.
- **Hooks:** Add pre/post-processing hooks for embeddings by subclassing and extending methods.

#### Example: Persistent Index
```python
class PersistentVectorIndex(InMemoryVectorIndex):
    def __init__(self, embedding_dim, storage_path):
        super().__init__(embedding_dim)
        self.storage_path = storage_path
        self._load_from_disk()
    def _load_from_disk(self):
        # Load index from disk
        pass
    def save(self):
        # Save index to disk
        pass
```

### Performance & Benchmarking
- Search for 10,000 vectors (384-dim) completes in ~3.6 ms (see `test_benchmark_search_performance`).
- Memory usage for 10,000 vectors is ~450 MiB (see `build_and_measure_index` with `memory_profiler`).
- All performance and memory targets for MVP are met.

### Testing & Profiling
- Run all tests: `PYTHONPATH=. pytest tests/services/test_vector_index.py`
- Run benchmarks: `pytest tests/services/test_vector_index.py -k 'benchmark' --benchmark-only`
- Profile memory usage: `python -m memory_profiler tests/services/test_vector_index.py [size] [dim]`
- All public methods and workflows are covered by unit and integration tests.

### Notes
- The index is designed for easy extension and integration with other storage or search backends.
- See code docstrings for further details and extension points.
