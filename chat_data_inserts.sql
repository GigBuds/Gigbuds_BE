-- =============================================
-- PostgreSQL INSERT Scripts for Gigbuds Chat System
-- Generated for: Conversations and Messages tables
-- =============================================

-- INSERT Sample Conversations
INSERT INTO public."Conversations"
(
    "Id",
    "IsEnabled",
    "CreatedAt",
    "UpdatedAt",
    "NameOne",
    "NameTwo",
    "AvatarOne",
    "AvatarTwo",
    "LastMessage",
    "LastMessageSenderName",
    "CreatorId"
)
VALUES
    -- Conversation 1: Job Application Discussion
    (
        1,
        true,
        '2024-01-15 10:30:00',
        '2024-01-15 14:25:00',
        'John Smith',
        'Tech Corp HR',
        'https://example.com/avatars/john_smith.jpg',
        'https://example.com/avatars/tech_corp.jpg',
        'Thank you for your interest! We''ll review your application.',
        'Tech Corp HR',
        '1'
    ),
    
    -- Conversation 2: Freelance Project
    (
        2,
        true,
        '2024-01-16 09:15:00',
        '2024-01-16 16:42:00',
        'Alice Johnson',
        'StartupXYZ',
        'https://example.com/avatars/alice_johnson.jpg',
        'https://example.com/avatars/startupxyz.jpg',
        'When can we schedule the initial meeting?',
        'Alice Johnson',
        '2'
    ),
    
    -- Conversation 3: Part-time Opportunity
    (
        3,
        true,
        '2024-01-17 11:20:00',
        '2024-01-17 11:20:00',
        'Mike Davis',
        'Local Business Inc',
        'https://example.com/avatars/mike_davis.jpg',
        'https://example.com/avatars/local_business.jpg',
        'Hi! I saw your job posting and I''m very interested.',
        'Mike Davis',
        '3'
    );

-- Reset sequence to continue from the last inserted ID
SELECT setval('public."Conversations_Id_seq"', (SELECT MAX("Id") FROM public."Conversations"));

-- =============================================

-- INSERT Sample Messages
INSERT INTO public."Messages"
(
    "Id",
    "IsEnabled",
    "CreatedAt",
    "UpdatedAt",
    "Content",
    "SendDate",
    "ConversationId",
    "AccountId"
)
VALUES
    -- Messages for Conversation 1 (Job Application)
    (
        1,
        true,
        '2024-01-15 10:30:00',
        '2024-01-15 10:30:00',
        'Hello! I''m very interested in the Software Developer position posted on your website.',
        '2024-01-15 10:30:00',
        1,
        1
    ),
    (
        2,
        true,
        '2024-01-15 11:45:00',
        '2024-01-15 11:45:00',
        'Thank you for reaching out! Could you please send us your resume and portfolio?',
        '2024-01-15 11:45:00',
        1,
        2
    ),
    (
        3,
        true,
        '2024-01-15 14:25:00',
        '2024-01-15 14:25:00',
        'Absolutely! I''ve attached my resume and portfolio. Looking forward to hearing from you.',
        '2024-01-15 14:25:00',
        1,
        1
    ),
    
    -- Messages for Conversation 2 (Freelance Project)
    (
        4,
        true,
        '2024-01-16 09:15:00',
        '2024-01-16 09:15:00',
        'Hi! I noticed your web design project posting. I have 5+ years of experience in React and modern web technologies.',
        '2024-01-16 09:15:00',
        2,
        2
    ),
    (
        5,
        true,
        '2024-01-16 10:30:00',
        '2024-01-16 10:30:00',
        'That sounds perfect! We''re looking for someone with React experience. What''s your hourly rate?',
        '2024-01-16 10:30:00',
        2,
        3
    ),
    (
        6,
        true,
        '2024-01-16 16:42:00',
        '2024-01-16 16:42:00',
        'My rate is $45/hour. When can we schedule the initial meeting to discuss the project details?',
        '2024-01-16 16:42:00',
        2,
        2
    ),
    
    -- Messages for Conversation 3 (Part-time)
    (
        7,
        true,
        '2024-01-17 11:20:00',
        '2024-01-17 11:20:00',
        'Hi! I saw your job posting for Part-time Marketing Assistant and I''m very interested. I have experience with social media marketing and content creation.',
        '2024-01-17 11:20:00',
        3,
        3
    );

-- Reset sequence to continue from the last inserted ID
SELECT setval('public."Messages_Id_seq"', (SELECT MAX("Id") FROM public."Messages"));

-- =============================================

-- INSERT Sample Conversation Members
INSERT INTO public."ConversationMembers"
(
    "IsEnabled",
    "CreatedAt",
    "UpdatedAt",
    "ConversationId",
    "AccountId",
    "IsAdmin",
    "JoinedDate",
    "LeaveDate"
)
VALUES
    -- Members for Conversation 1
    (true, '2024-01-15 10:29:00', '2024-01-15 10:29:00', 1, 1, true, '2024-01-15 10:29:00', NULL),
    (true, '2024-01-15 10:29:00', '2024-01-15 10:29:00', 1, 2, false, '2024-01-15 10:29:00', NULL),

    -- Members for Conversation 2
    (true, '2024-01-16 09:14:00', '2024-01-16 09:14:00', 2, 2, true, '2024-01-16 09:14:00', NULL),
    (true, '2024-01-16 09:14:00', '2024-01-16 09:14:00', 2, 3, false, '2024-01-16 09:14:00', NULL),

    -- Members for Conversation 3
    (true, '2024-01-17 11:19:00', '2024-01-17 11:19:00', 3, 2, true, '2024-01-17 11:19:00', NULL),
    (true, '2024-01-17 11:19:00', '2024-01-17 11:19:00', 3, 3, false, '2024-01-17 11:19:00', NULL);

-- =============================================
-- VERIFICATION QUERIES
-- =============================================

-- Verify Conversations data
SELECT 
    "Id",
    "NameOne",
    "NameTwo",
    "LastMessage",
    "LastMessageSenderName",
    "CreatedAt"
FROM public."Conversations"
ORDER BY "Id";

-- Verify Messages data with conversation details
SELECT 
    m."Id",
    m."Content",
    m."SendDate",
    m."ConversationId",
    c."NameOne",
    c."NameTwo",
    m."AccountId"
FROM public."Messages" m
INNER JOIN public."Conversations" c ON m."ConversationId" = c."Id"
ORDER BY m."ConversationId", m."SendDate";

-- Count messages per conversation
SELECT 
    c."Id" AS "ConversationId",
    c."NameOne" || ' <-> ' || c."NameTwo" AS "ConversationParticipants",
    COUNT(m."Id") AS "MessageCount"
FROM public."Conversations" c
LEFT JOIN public."Messages" m ON c."Id" = m."ConversationId"
GROUP BY c."Id", c."NameOne", c."NameTwo"
ORDER BY c."Id";

-- Verify Conversation Members
SELECT
    cm."ConversationId",
    c."NameOne" || ' <-> ' || c."NameTwo" AS "Conversation",
    cm."AccountId",
    cm."IsAdmin",
    cm."JoinedDate"
FROM public."ConversationMembers" cm
INNER JOIN public."Conversations" c ON cm."ConversationId" = c."Id"
ORDER BY cm."ConversationId", cm."AccountId";

-- =============================================
-- POSTGRESQL COMPATIBILITY NOTES:
-- 1. Account ID values are set to 1-3 as requested
-- 2. Update avatar URLs to match your storage system
-- 3. Creator ID values are set to 1, 2, or 3 to reflect the conversation creator
-- 4. All timestamps are in UTC format as per BaseEntity configuration
-- 5. Content field has 1000 character limit as per MessageConfiguration
-- 6. Ensure ApplicationUser records with IDs 1, 2, and 3 exist before running this script
-- 7. Uses case-sensitive, double-quoted identifiers (e.g., public."Conversations") as specified
-- 8. Boolean values use 'true'/'false'
-- 9. Sequence names may vary - adjust 'public."Conversations_Id_seq"' to match your schema
-- 10. If sequences don't exist, PostgreSQL will auto-generate IDs starting from the next available number
-- ============================================= 