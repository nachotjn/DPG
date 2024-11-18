drop schema public cascade;
create schema public;

CREATE TABLE Board (
    boardId SERIAL PRIMARY KEY,
    sequence INT NOT NULL,
    type VARCHAR(255) NOT NULL
);


CREATE TABLE User (
    userId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    isAdmin BOOLEAN NOT NULL DEFAULT FALSE
);


CREATE TABLE BoardsForUser (
    boardsForUserId SERIAL PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES User(userId) ON DELETE CASCADE,
    board_id INT NOT NULL REFERENCES Board(boardId) ON DELETE CASCADE
);
