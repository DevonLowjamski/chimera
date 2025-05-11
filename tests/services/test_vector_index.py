import numpy as np
import pytest

from src.chunking.text_chunk import TextChunk
from src.services.vector_index import InMemoryVectorIndex


@pytest.fixture
def sample_chunk():
    return TextChunk(content="test", source_document_id="doc1")


@pytest.fixture
def sample_embedding():
    return [0.1, 0.2, 0.3]


@pytest.fixture
def index():
    return InMemoryVectorIndex(embedding_dim=3)


def test_add_and_get_embedding(index, sample_chunk, sample_embedding):
    index.add(sample_embedding, sample_chunk)
    emb = index.get_embedding(sample_chunk.chunk_id)
    assert np.allclose(emb, np.array(sample_embedding, dtype=np.float32))
    assert len(index) == 1


def test_get_chunk_ref(index, sample_chunk, sample_embedding):
    index.add(sample_embedding, sample_chunk)
    ref = index.get_chunk_ref(sample_chunk.chunk_id)
    assert isinstance(ref, TextChunk)
    assert ref.chunk_id == sample_chunk.chunk_id


def test_add_with_id(index, sample_embedding):
    chunk_id = "custom-id"
    index.add(sample_embedding, chunk_id)
    assert index.get_embedding(chunk_id) is not None
    assert index.get_chunk_ref(chunk_id) == chunk_id


def test_remove(index, sample_chunk, sample_embedding):
    index.add(sample_embedding, sample_chunk)
    removed = index.remove(sample_chunk.chunk_id)
    assert removed
    assert index.get_embedding(sample_chunk.chunk_id) is None
    assert len(index) == 0


def test_remove_nonexistent(index):
    assert not index.remove("does-not-exist")


def test_add_wrong_dim(index, sample_chunk):
    with pytest.raises(ValueError):
        index.add([1.0, 2.0], sample_chunk)  # Wrong dimension


def test_search_basic(index, sample_chunk, sample_embedding):
    # Add three orthogonal and one identical embedding
    index.add([1, 0, 0], "a")
    index.add([0, 1, 0], "b")
    index.add([0, 0, 1], "c")
    index.add([1, 0, 0], "d")
    # Query identical to [1, 0, 0]
    results = index.search([1, 0, 0], top_k=2)
    assert results[0]["chunk_id"] in ("a", "d")
    assert results[1]["chunk_id"] in ("a", "d")
    assert results[0]["score"] == pytest.approx(1.0)
    # Query orthogonal to all but one
    results = index.search([0, 1, 0], top_k=1)
    assert results[0]["chunk_id"] == "b"
    assert results[0]["score"] == pytest.approx(1.0)


def test_search_empty_index():
    idx = InMemoryVectorIndex(3)
    assert idx.search([1, 0, 0]) == []


def test_search_wrong_dim(index):
    index.add([1, 0, 0], "a")
    with pytest.raises(ValueError):
        index.search([1, 0])


def test_search_zero_query(index):
    index.add([1, 0, 0], "a")
    with pytest.raises(ValueError):
        index.search([0, 0, 0])


def test_add_duplicate_chunk_id(index, sample_chunk, sample_embedding):
    index.add(sample_embedding, sample_chunk)
    with pytest.raises(ValueError, match="already exists"):
        index.add(sample_embedding, sample_chunk)


def test_add_empty_embedding(index, sample_chunk):
    with pytest.raises(ValueError, match="dimension"):
        index.add([], sample_chunk)


def test_add_non_numeric_embedding(index, sample_chunk):
    with pytest.raises(ValueError, match="numeric"):
        index.add([1.0, "a", 3.0], sample_chunk)


def test_add_nan_inf_embedding(index, sample_chunk):
    import math

    with pytest.raises(ValueError, match="NaN"):
        index.add([1.0, math.nan, 3.0], sample_chunk)
    with pytest.raises(ValueError, match="infinite"):
        index.add([1.0, float("inf"), 3.0], sample_chunk)


def test_remove_after_duplicate_attempt(index, sample_chunk, sample_embedding):
    index.add(sample_embedding, sample_chunk)
    try:
        index.add(sample_embedding, sample_chunk)
    except ValueError:
        pass
    removed = index.remove(sample_chunk.chunk_id)
    assert removed
    assert index.get_embedding(sample_chunk.chunk_id) is None


def test_search_result_format(index, sample_chunk, sample_embedding):
    index.add([1, 0, 0], sample_chunk)
    index.add([0, 1, 0], "b")
    results = index.search([1, 0, 0], top_k=2)
    assert isinstance(results, list)
    assert all(isinstance(r, dict) for r in results)
    for r in results:
        assert "score" in r and "chunk_id" in r and "content" in r and "metadata" in r
    # Check correct content/metadata for TextChunk
    assert results[0]["chunk_id"] == sample_chunk.chunk_id
    assert results[0]["content"] == sample_chunk.content
    assert results[0]["metadata"] == sample_chunk.metadata
    # Check correct content/metadata for string chunk_ref
    assert results[1]["chunk_id"] == "b"
    assert results[1]["content"] is None
    assert results[1]["metadata"] == {}


def test_search_sorted_by_score(index):
    index.add([1, 0, 0], "a")
    index.add([0.9, 0.1, 0], "b")
    index.add([0, 1, 0], "c")
    results = index.search([1, 0, 0], top_k=3)
    scores = [r["score"] for r in results]
    assert scores == sorted(scores, reverse=True)
