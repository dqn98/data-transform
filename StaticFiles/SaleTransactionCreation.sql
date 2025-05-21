CREATE TABLE f_SaleTransactions_dummy (
                                          id BIGINT PRIMARY KEY,
                                          memberkey INTEGER,
                                          total_amount MONEY,
                                          terminal VARCHAR(20),
                                          transaction_datetime TIMESTAMP WITHOUT TIME ZONE
);

INSERT INTO f_SaleTransactions_dummy (id, memberkey, total_amount, terminal, transaction_datetime) VALUES
(2998, 43933, '$79.33', '47T00362', '2025-04-30 14:03:35'),
(2999, 43933, '$2.50', '47T00362', '2025-04-30 14:03:05'),
(3000, 43933, '$2.52', '47T00362', '2025-04-30 14:05:44'),
(3001, 43933, '$582.25', '47T00362', '2025-04-30 14:05:53'),
(3002, 43933, '$82.80', '47T00362', '2025-04-30 14:05:43'),
(3003, 43946, '$21.50', '47T00363', '2025-04-25 15:25:54'),
(3004, 43773, '$6.67', '47T00316', '2025-04-25 14:13:35'),
(3005, 43773, '$77.77', '47T00316', '2025-04-25 14:13:04'),
(3006, 43933, '$146.88', '47T00362', '2025-04-25 13:54:03'),
(3007, 43948, '$55.55', '47T00364', '2025-04-25 13:05:04'),
(3008, 43773, '$99.77', '47T00316', '2025-04-25 13:04:55'),
(3009, 42490, '$5.93', '47T00096', '2025-04-25 15:24:45'),
(3010, 42490, '$10.08', '47T00096', '2025-04-25 15:24:24'),
(3011, 43773, '$67.97', '47T00316', '2025-04-25 15:14:51'),
(3012, 43773, '$0.09', '47T00316', '2025-04-25 15:14:24'),
(3013, 43773, '$77.77', '47T00316', '2025-04-24 20:05:32'),
(3014, 42490, '$13.05', '47T00096', '2025-04-24 14:20:03');


CREATE TABLE terminal_data (
                               terminalid VARCHAR(20),
                               D_EftterminalKey INT,
                               D_VehicleTypeKey INT,
                               D_MemberKey INT
);

INSERT INTO terminal_data (
    terminalid, "D_EftterminalKey", "D_VehicleTypeKey", "D_MemberKey") VALUES
                                                                           ('47T00098', 81228, 55, 71485),
                                                                           ('47T00096', 81001, 62, 40236),
                                                                           ('47T00062', 79176, 62, 69063),
                                                                           ('47T00066', 80151, 55, 37380);

SELECT D_VehicleTypeKey FROM terminal_data WHERE terminalid = '47T00066' limit 1
SELECT s."Value" FROM (     SELECT D_VehicleTypeKey as []] FROM terminal_data WHERE terminalid = '47T00066' limit 1 ) AS s LIMIT 1

drop table if exists user_events
drop table if exists raw_user_events
drop table if exists terminal_data


select * from user_events
select * from raw_user_events
select * from terminal_data