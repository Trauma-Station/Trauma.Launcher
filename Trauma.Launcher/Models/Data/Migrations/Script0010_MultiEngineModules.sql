-- Add Engine field to EngineModule
ALTER TABLE EngineModule ADD COLUMN Engine TEXT NOT NULL DEFAULT "OldToolbox";

-- then change its primary key
CREATE TABLE EngineModule2 (
	Engine TEXT NOT NULL,
    Name TEXT NOT NULL,
    Version TEXT NOT NULL,

    PRIMARY KEY (Engine, Name, Version)
);

INSERT INTO EngineModule2 (Engine, Name, Version) SELECT Engine, Name, Version FROM EngineModule;
DROP TABLE EngineModule;
ALTER TABLE EngineModule2 RENAME TO EngineModule;
