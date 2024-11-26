CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


CREATE TABLE Player (
    playerId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone character varying(50),
    password VARCHAR(255) NOT NULL,
    isAdmin BOOLEAN NOT NULL DEFAULT FALSE,
    isActive BOOLEAN NOT NULL DEFAULT TRUE,
    balance NUMERIC(10, 2) NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE Transaction (
    transactionId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    playerId UUID NOT NULL REFERENCES Player(playerId) ON DELETE CASCADE,
    transactionType VARCHAR(50) NOT NULL, 
    amount NUMERIC(10, 2) NOT NULL,
    balanceAfterTransaction NUMERIC(10, 2) NOT NULL, 
    description TEXT,
    isConfirmed BOOLEAN NOT NULL DEFAULT FALSE, 
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Game (
    gameId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    weekNumber INT NOT NULL,
    year INT NOT NULL,
    winningNumbers INT[] CHECK (array_length(winningNumbers, 1) = 3),
    isComplete BOOLEAN NOT NULL DEFAULT FALSE, 
    prizeSum NUMERIC(10, 2) DEFAULT 0, 
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE Board (
    boardId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    playerId UUID NOT NULL REFERENCES Player(playerId) ON DELETE CASCADE,
    gameId UUID NOT NULL REFERENCES Game(gameId) ON DELETE CASCADE,
    numbers INT[] NOT NULL CHECK (array_length(numbers, 1) BETWEEN 5 AND 8),
    isAutoPlay BOOLEAN NOT NULL DEFAULT FALSE,
    autoplayWeeks INT DEFAULT 0,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);



CREATE TABLE Winner (
    winnerId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    playerId UUID NOT NULL REFERENCES Player(playerId) ON DELETE CASCADE,
    gameId UUID NOT NULL REFERENCES Game(gameId) ON DELETE CASCADE,
    boardId UUID NOT NULL REFERENCES Board(boardId) ON DELETE CASCADE,
    winningAmount NUMERIC(10, 2) NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

