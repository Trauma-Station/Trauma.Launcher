CREATE TABLE Login (
    AuthServer Text NOT NULL,
    UserId TEXT NOT NULL, -- GUID
    -- Name of the auth server used
    UserName TEXT NOT NULL,
    Token TEXT NOT NULL,
    Expires DATETIME NOT NULL,

    PRIMARY KEY (AuthServer, UserId)
);

CREATE TABLE Config (
    Key TEXT PRIMARY KEY NOT NULL,
    Value
);

CREATE TABLE FavoriteServer (
    Address TEXT PRIMARY KEY NOT NULL,
    Name TEXT
);

CREATE TABLE ServerContent (
    ForkId TEXT PRIMARY KEY NOT NULL,
    CurrentVersion TEXT NOT NULL,
    CurrentHash TEXT,
    CurrentEngineVersion TEXT NOT NULL,
    DiskId INTEGER NOT NULL
);

CREATE TABLE EngineInstallation (
	Engine TEXT NOT NULL,
    Version TEXT PRIMARY KEY NOT NULL,
    Signature TEXT NOT NULL
);
