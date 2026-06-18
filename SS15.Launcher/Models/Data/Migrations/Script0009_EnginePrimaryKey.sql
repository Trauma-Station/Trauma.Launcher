-- Change PrimaryKey from Version to (Engine, Version) i forgor to
-- have to do all this sqlite is incredible

CREATE TABLE EngineInstallation2 (
	Engine TEXT NOT NULL,
    Version TEXT NOT NULL,
    Signature TEXT NOT NULL,

    PRIMARY KEY (Engine, Version)
);

INSERT INTO EngineInstallation2 (Engine, Version, Signature) SELECT Engine, Version, Signature FROM EngineInstallation;
DROP TABLE EngineInstallation;
ALTER TABLE EngineInstallation2 RENAME TO EngineInstallation;
