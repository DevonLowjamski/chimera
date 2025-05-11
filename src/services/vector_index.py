from typing import Dict, List, Optional, Tuple, Union

import numpy as np

from src.chunking.text_chunk import TextChunk


class InMemoryVectorIndex:
    """
    In-memory vector index for fast access to embedding vectors and their associated TextChunks.
    Designed for extensibility to persistent or distributed implementations.
    """

    def __init__(self, embedding_dim: int):
        """
        Initialize the index.
        Args:
            embedding_dim (int): The dimensionality of the embedding vectors.
        """
        self.embedding_dim = embedding_dim
        self.embeddings: np.ndarray = np.empty((0, embedding_dim), dtype=np.float32)
        self.chunk_refs: List[Union[TextChunk, str]] = []  # Store TextChunk or chunk_id
        self.id_to_index: Dict[str, int] = {}  # Optional: fast lookup by chunk_id

    def add(self, embedding: List[float], chunk_ref: Union[TextChunk, str]) -> None:
        """
        Add an embedding and its reference to the index.
        Args:
            embedding (List[float]): The embedding vector.
            chunk_ref (TextChunk or str): The associated TextChunk or its ID.
        """
        # Validate embedding type
        if not isinstance(embedding, (list, np.ndarray)):
            raise ValueError("Embedding must be a list or numpy array.")
        if len(embedding) != self.embedding_dim:
            raise ValueError(
                f"Embedding dimension {len(embedding)} does not match index dimension "
                f"{self.embedding_dim}."
            )
        # Validate all elements are numeric
        if not all(isinstance(x, (int, float)) for x in embedding):
            raise ValueError("Embedding contains non-numeric values.")
        # Validate all elements are finite
        arr = np.array(embedding, dtype=np.float32)
        if not np.all(np.isfinite(arr)):
            raise ValueError("Embedding contains NaN or infinite values.")
        # Check for duplicate chunk_id
        chunk_id = chunk_ref.chunk_id if isinstance(chunk_ref, TextChunk) else chunk_ref
        if chunk_id in self.id_to_index:
            raise ValueError(f"Chunk ID '{chunk_id}' already exists in the index.")
        self.embeddings = np.vstack([self.embeddings, arr])
        self.chunk_refs.append(chunk_ref)
        self.id_to_index[chunk_id] = len(self.chunk_refs) - 1

    def get_embedding(self, chunk_id: str) -> Optional[np.ndarray]:
        """
        Retrieve the embedding vector for a given chunk ID.
        Args:
            chunk_id (str): The ID of the chunk.
        Returns:
            np.ndarray or None: The embedding vector, or None if not found.
        """
        idx = self.id_to_index.get(chunk_id)
        if idx is not None:
            return self.embeddings[idx]
        return None

    def get_chunk_ref(self, chunk_id: str) -> Optional[Union[TextChunk, str]]:
        """
        Retrieve the chunk reference for a given chunk ID.
        Args:
            chunk_id (str): The ID of the chunk.
        Returns:
            TextChunk, str, or None: The chunk reference, or None if not found.
        """
        idx = self.id_to_index.get(chunk_id)
        if idx is not None:
            return self.chunk_refs[idx]
        return None

    def remove(self, chunk_id: str) -> bool:
        """
        Remove an embedding and its reference by chunk ID.
        Args:
            chunk_id (str): The ID of the chunk to remove.
        Returns:
            bool: True if removed, False if not found.
        """
        idx = self.id_to_index.pop(chunk_id, None)
        if idx is None:
            return False
        try:
            self.embeddings = np.delete(self.embeddings, idx, axis=0)
            self.chunk_refs.pop(idx)
            # Rebuild id_to_index for consistency
            self.id_to_index = {
                (ref.chunk_id if isinstance(ref, TextChunk) else ref): i
                for i, ref in enumerate(self.chunk_refs)
            }
            return True
        except Exception as e:
            # Rollback not implemented for MVP, but log error
            raise RuntimeError(f"Failed to remove chunk_id '{chunk_id}': {e}")

    def __len__(self) -> int:
        """Return the number of vectors in the index."""
        return len(self.chunk_refs)

    def search(
        self, query: List[float], top_k: int = 5
    ) -> List[Tuple[float, Union[TextChunk, str]]]:
        """
        Search for the top_k most similar embeddings to the query using cosine similarity.
        Args:
            query (List[float]): The query embedding vector.
            top_k (int): Number of top results to return.
        Returns:
            List of (score, chunk_ref) tuples, sorted by descending similarity.
        """
        if len(query) != self.embedding_dim:
            raise ValueError(
                f"Query dimension {len(query)} does not match index dimension {self.embedding_dim}."
            )
        if len(self.embeddings) == 0:
            return []
        query_vec = np.array(query, dtype=np.float32)
        # Normalize query and embeddings
        query_norm = np.linalg.norm(query_vec)
        if query_norm == 0:
            raise ValueError("Query vector must not be zero.")
        emb_norms = np.linalg.norm(self.embeddings, axis=1)
        # Avoid division by zero
        emb_norms[emb_norms == 0] = 1e-10
        # Cosine similarity
        sims = np.dot(self.embeddings, query_vec) / (emb_norms * query_norm)
        # Get top_k indices
        top_indices = np.argsort(-sims)[:top_k]
        results = [(float(sims[i]), self.chunk_refs[i]) for i in top_indices]
        return results
