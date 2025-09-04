-- init.sql

-- Garante extensão para gerar UUIDs
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

CREATE TABLE IF NOT EXISTS todo (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Title VARCHAR(150) NOT NULL,
    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE
);
