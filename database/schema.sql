CREATE TABLE users (
    user_id       SERIAL PRIMARY KEY,
    username      VARCHAR(50)  NOT NULL UNIQUE,
    password_hash VARCHAR(200) NOT NULL,
    full_name     VARCHAR(100) NOT NULL
);

CREATE TABLE donors (
    donor_id      SERIAL PRIMARY KEY,
    national_id   VARCHAR(20)  NOT NULL UNIQUE,
    full_name     VARCHAR(100) NOT NULL,
    phone         VARCHAR(20)  NOT NULL,
    date_of_birth DATE         NOT NULL,
    district      VARCHAR(50)  NOT NULL,
    blood_group   VARCHAR(3)   NOT NULL
                  CHECK (blood_group IN ('A+','A-','B+','B-','AB+','AB-','O+','O-'))
);

CREATE TABLE donations (
    donation_id   SERIAL PRIMARY KEY,
    donor_id      INT  NOT NULL REFERENCES donors(donor_id) ON DELETE RESTRICT,
    donation_date DATE NOT NULL,
    place         VARCHAR(100) NOT NULL,
    amount_ml     INT  NOT NULL CHECK (amount_ml BETWEEN 100 AND 600)
);

CREATE TABLE settings (
    setting_key   VARCHAR(50) PRIMARY KEY,
    setting_value VARCHAR(50) NOT NULL
);

INSERT INTO settings VALUES ('waiting_period_months', '3');