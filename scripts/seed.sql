-- Admin: admin@hmz.com / Admin@123
-- User : janedoe@hmz.com / userpass
INSERT INTO app_users (email, first_name, last_name, avatar_url, password_salt_hex, password_hash_hex, iterations)
VALUES
('admin@hmz.com','Admin','User',NULL,'29062ada195d9f96910068beec8805d4','e7f0bc686519e9352fa404ac7faffad04aff1ac350b198529a62ef00afec1aec',100000),
('janedoe@hmz.com','Jane','Doe',NULL,'78d0067a936ed34b8054987c017b5614','f324494e7dc25f3a033c49f9fa13f8cf2b9fd1174c2e8fb2e3124d8624473c17',100000);
