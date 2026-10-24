IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    CREATE TABLE [Authors] (
        [AuthorId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NULL,
        [Bio] nvarchar(500) NULL,
        CONSTRAINT [PK_Authors] PRIMARY KEY ([AuthorId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    CREATE TABLE [Categories] (
        [CategoryId] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Description] nvarchar(250) NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    CREATE TABLE [Books] (
        [BookId] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [ISBN] nvarchar(20) NULL,
        [PublishedYear] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [AuthorId] int NOT NULL,
        [CategoryId] int NOT NULL,
        CONSTRAINT [PK_Books] PRIMARY KEY ([BookId]),
        CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([AuthorId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Books_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AuthorId', N'Bio', N'Email', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] ON;
    EXEC(N'INSERT INTO [Authors] ([AuthorId], [Bio], [Email], [Name])
    VALUES (1, N''Nhà văn nổi tiếng của thiếu nhi Việt Nam'', N''anhnn@nxbtre.com.vn'', N''Nguyễn Nhật Ánh''),
    (2, N''Nhà văn hiện thực xuất sắc đầu thế kỷ 20'', N''namcao@classic.vn'', N''Nam Cao''),
    (3, N''Tác giả bộ tiểu thuyết Harry Potter đình đám'', N''jkrowling@pottermore.com'', N''J.K. Rowling'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AuthorId', N'Bio', N'Email', N'Name') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CategoryId', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([CategoryId], [Description], [Name])
    VALUES (1, N''Sách dành cho lứa tuổi thanh thiếu niên'', N''Văn học thiếu nhi''),
    (2, N''Tác phẩm văn học hiện thực xã hội'', N''Hiện thực phê phán''),
    (3, N''Tiểu thuyết kỳ ảo và phép thuật'', N''Giả tưởng / Fantasy'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CategoryId', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'BookId', N'AuthorId', N'CategoryId', N'ISBN', N'Price', N'PublishedYear', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
        SET IDENTITY_INSERT [Books] ON;
    EXEC(N'INSERT INTO [Books] ([BookId], [AuthorId], [CategoryId], [ISBN], [Price], [PublishedYear], [Title])
    VALUES (1, 1, 1, N''978-604-1-00001-1'', 85000.0, 2008, N''Cho tôi xin một vé đi tuổi thơ''),
    (2, 1, 1, N''978-604-1-00002-8'', 110000.0, 1990, N''Mắt biếc''),
    (3, 2, 2, N''978-604-2-00003-5'', 65000.0, 1941, N''Chí Phèo''),
    (4, 3, 3, N''978-074-7-53269-9'', 135000.0, 1997, N''Harry Potter và Hòn đá Phù thủy'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'BookId', N'AuthorId', N'CategoryId', N'ISBN', N'Price', N'PublishedYear', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
        SET IDENTITY_INSERT [Books] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    CREATE INDEX [IX_Books_AuthorId] ON [Books] ([AuthorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    CREATE INDEX [IX_Books_CategoryId] ON [Books] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261024035658_InitialLibraryDB'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261024035658_InitialLibraryDB', N'8.0.10');
END;
GO

COMMIT;
GO

