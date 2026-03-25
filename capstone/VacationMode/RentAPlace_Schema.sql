Build started...
Build succeeded.
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

CREATE TABLE [Features] (
    [FeatureId] int NOT NULL IDENTITY,
    [FeatureName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Features] PRIMARY KEY ([FeatureId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Properties] (
    [PropertyId] int NOT NULL IDENTITY,
    [OwnerId] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [PropertyType] nvarchar(max) NOT NULL,
    [PricePerNight] decimal(18,2) NOT NULL,
    [MaxGuests] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY ([PropertyId]),
    CONSTRAINT [FK_Properties_Users_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Messages] (
    [MessageId] int NOT NULL IDENTITY,
    [SenderId] int NOT NULL,
    [ReceiverId] int NOT NULL,
    [PropertyId] int NOT NULL,
    [MessageText] nvarchar(max) NOT NULL,
    [SentAt] datetime2 NOT NULL,
    [IsRead] bit NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY ([MessageId]),
    CONSTRAINT [FK_Messages_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [PropertyFeatures] (
    [PropertyFeatureId] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [FeatureId] int NOT NULL,
    CONSTRAINT [PK_PropertyFeatures] PRIMARY KEY ([PropertyFeatureId]),
    CONSTRAINT [FK_PropertyFeatures_Features_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [Features] ([FeatureId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PropertyFeatures_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE
);
GO

CREATE TABLE [PropertyImages] (
    [ImageId] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [IsPrimary] bit NOT NULL,
    CONSTRAINT [PK_PropertyImages] PRIMARY KEY ([ImageId]),
    CONSTRAINT [FK_PropertyImages_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Reservations] (
    [ReservationId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [PropertyId] int NOT NULL,
    [CheckInDate] datetime2 NOT NULL,
    [CheckOutDate] datetime2 NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [ReservationStatus] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([ReservationId]),
    CONSTRAINT [FK_Reservations_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Reviews] (
    [ReviewId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [PropertyId] int NOT NULL,
    [Rating] int NOT NULL,
    [Comment] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([ReviewId]),
    CONSTRAINT [FK_Reviews_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([PropertyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reviews_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Messages_PropertyId] ON [Messages] ([PropertyId]);
GO

CREATE INDEX [IX_Messages_ReceiverId] ON [Messages] ([ReceiverId]);
GO

CREATE INDEX [IX_Messages_SenderId] ON [Messages] ([SenderId]);
GO

CREATE INDEX [IX_Properties_OwnerId] ON [Properties] ([OwnerId]);
GO

CREATE INDEX [IX_PropertyFeatures_FeatureId] ON [PropertyFeatures] ([FeatureId]);
GO

CREATE INDEX [IX_PropertyFeatures_PropertyId] ON [PropertyFeatures] ([PropertyId]);
GO

CREATE INDEX [IX_PropertyImages_PropertyId] ON [PropertyImages] ([PropertyId]);
GO

CREATE INDEX [IX_Reservations_PropertyId] ON [Reservations] ([PropertyId]);
GO

CREATE INDEX [IX_Reservations_UserId] ON [Reservations] ([UserId]);
GO

CREATE INDEX [IX_Reviews_PropertyId] ON [Reviews] ([PropertyId]);
GO

CREATE INDEX [IX_Reviews_UserId] ON [Reviews] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260313041351_First', N'8.0.6');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260315220239_UpdateModels', N'8.0.6');
GO

COMMIT;
GO


