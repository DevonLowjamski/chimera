import os
from pathlib import Path
from typing import Optional

from pydantic import BaseModel


class AppConfig(BaseModel):
    app_name: str = "ResearchContextMCP"
    debug_mode: bool = False
    log_level: str = "INFO"
    data_dir: Optional[Path] = None

    @classmethod
    def from_env(cls) -> "AppConfig":
        """Load configuration from environment variables"""
        return cls(
            app_name=os.getenv("APP_NAME", "ResearchContextMCP"),
            debug_mode=os.getenv("DEBUG_MODE", "False").lower() == "true",
            log_level=os.getenv("LOG_LEVEL", "INFO"),
            data_dir=Path(os.getenv("DATA_DIR", "./data"))
            if os.getenv("DATA_DIR")
            else None,
        )


# Default config instance
config = AppConfig.from_env()
