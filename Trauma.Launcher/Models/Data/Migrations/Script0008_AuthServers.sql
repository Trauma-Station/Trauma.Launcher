-- Custom auth servers added by the user
CREATE TABLE AuthServer (
    Name TEXT NOT NULL PRIMARY KEY,
    AccountBaseUrl TEXT NOT NULL,
    AuthUrl TEXT NOT NULL,

    -- Fields can't be empty
    CONSTRAINT NameNotEmpty CHECK (Name <> ''),
    CONSTRAINT AccountNotEmpty CHECK (AccountBaseUrl <> '')
    CONSTRAINT UrlNotEmpty CHECK (AuthUrl <> '')
);
