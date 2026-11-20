-- ============================================================
-- LifeDrop – sample data  (run ONCE after schema.sql)
-- All names, IDs and phone numbers are fictional.
-- ============================================================

INSERT INTO donors (national_id, full_name, phone, date_of_birth, district, blood_group) VALUES
('1199780012340001', 'Alice Uwimana',      '+250788100001', '1990-03-14', 'Gasabo',     'A+'),
('1199880012340002', 'Bernard Nkurunziza', '+250788100002', '1988-07-22', 'Kicukiro',   'B+'),
('1200080012340003', 'Claudine Mukamana',  '+250788100003', '2000-11-05', 'Nyarugenge', 'O+'),
('1199580012340004', 'David Habimana',     '+250788100004', '1995-01-30', 'Musanze',    'AB+'),
('1199280012340005', 'Esperance Ingabire', '+250788100005', '1992-06-18', 'Rubavu',     'A-'),
('1198780012340006', 'Francois Bizimana',  '+250788100006', '1987-09-09', 'Huye',       'B-'),
('1200380012340007', 'Grace Uwase',        '+250788100007', '2003-04-25', 'Rwamagana',  'O-'),
('1199180012340008', 'Herve Niyonzima',    '+250788100008', '1991-12-03', 'Kayonza',    'AB-'),
('1199680012340009', 'Immaculee Mukunde',  '+250788100009', '1996-08-17', 'Gasabo',     'O+'),
('1198580012340010', 'Jean Damascene',     '+250788100010', '1985-02-28', 'Kicukiro',   'A+'),
('1200180012340011', 'Keza Iradukunda',    '+250788100011', '2001-05-11', 'Nyarugenge', 'B+'),
('1199380012340012', 'Leon Nshimiyimana',  '+250788100012', '1993-10-07', 'Musanze',    'O+'),
('1199780012340013', 'Marie Ange Umubyeyi','+250788100013', '1997-03-22', 'Rubavu',     'A+'),
('1198980012340014', 'Noel Hakizimana',    '+250788100014', '1989-07-14', 'Huye',       'AB+'),
('1200480012340015', 'Olive Nyiraneza',    '+250788100015', '2004-01-19', 'Rwamagana',  'O-');

-- Donations spread across the last 8 months so the charts have data
-- donor_id 1  (Alice,      A+)
INSERT INTO donations (donor_id, donation_date, place, amount_ml) VALUES
(1,  CURRENT_DATE - INTERVAL '7 months', 'Kigali Blood Bank',        450),
(1,  CURRENT_DATE - INTERVAL '3 months', 'Gasabo Health Centre',     500),

-- donor_id 2  (Bernard,    B+)
(2,  CURRENT_DATE - INTERVAL '6 months', 'King Faisal Hospital',     450),
(2,  CURRENT_DATE - INTERVAL '2 months', 'Kicukiro District Hospital',450),

-- donor_id 3  (Claudine,   O+)
(3,  CURRENT_DATE - INTERVAL '5 months', 'Kigali Blood Bank',        400),

-- donor_id 4  (David,      AB+)
(4,  CURRENT_DATE - INTERVAL '4 months', 'Musanze District Hospital', 500),
(4,  CURRENT_DATE - INTERVAL '1 month',  'Musanze District Hospital', 450),

-- donor_id 5  (Esperance,  A-)
(5,  CURRENT_DATE - INTERVAL '8 months', 'Rubavu Health Centre',     350),
(5,  CURRENT_DATE - INTERVAL '4 months', 'Rubavu Health Centre',     400),

-- donor_id 6  (Francois,   B-)
(6,  CURRENT_DATE - INTERVAL '3 months', 'Huye University Hospital', 450),

-- donor_id 8  (Herve,      AB-)
(8,  CURRENT_DATE - INTERVAL '6 months', 'Kayonza Health Centre',    500),
(8,  CURRENT_DATE - INTERVAL '2 months', 'Kayonza Health Centre',    450),

-- donor_id 9  (Immaculee,  O+)
(9,  CURRENT_DATE - INTERVAL '5 months', 'Kigali Blood Bank',        400),

-- donor_id 10 (Jean,       A+)
(10, CURRENT_DATE - INTERVAL '7 months', 'Kicukiro District Hospital',500),
(10, CURRENT_DATE - INTERVAL '3 months', 'Kigali Blood Bank',        450),

-- donor_id 12 (Leon,       O+)
(12, CURRENT_DATE - INTERVAL '2 months', 'Musanze District Hospital', 400),

-- donor_id 14 (Noel,       AB+)
(14, CURRENT_DATE - INTERVAL '1 month',  'Huye University Hospital', 500);
-- donors 7 (Grace O-), 11 (Keza B+), 13 (Marie A+), 15 (Olive O-) have never donated → always Ready
