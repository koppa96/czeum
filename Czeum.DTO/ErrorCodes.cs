namespace Czeum.DTO
{
    /// <summary>
    /// A list of error codes that are sent by the GameHub to their error handling clients.
    /// </summary>
    public static class ErrorCodes
    {
        //Lobby management related errors
        public const string InvalidBoardSize = nameof(InvalidBoardSize);
        public const string AlreadyInLobby = nameof(AlreadyInLobby);
        public const string AlreadyQueuing = nameof(AlreadyQueuing);
        public const string NoSuchLobby = nameof(NoSuchLobby);
        public const string NoRightToChange = nameof(NoRightToChange);
        public const string CouldNotJoinLobby = nameof(CouldNotJoinLobby);
        public const string InvalidLobbyType = nameof(InvalidLobbyType);
        public const string NotEnoughPlayers = nameof(NotEnoughPlayers);
        public const string CannotSendMessage = nameof(CannotSendMessage);
        
        //Friend management related errors
        public const string NoSuchUser = nameof(NoSuchUser);
        public const string AlreadyFriends = nameof(AlreadyFriends);
        public const string AlreadyRequested = nameof(AlreadyRequested);
        public const string NoSuchRequest = nameof(NoSuchRequest);
        public const string NoSuchFriendship = nameof(NoSuchFriendship);
        
        //Game related errors
        public const string NoSuchMatch = nameof(NoSuchMatch);
        public const string NotYourTurn = nameof(NotYourTurn);
        public const string NotYourMatch = nameof(NotYourMatch);
        public const string GameNotSupported = nameof(GameNotSupported);
        public const string MatchEnded = nameof(MatchEnded);
        
        //User management errors
        public const string UsernameAlreadyTaken = nameof(UsernameAlreadyTaken);
        public const string PasswordsNotMatching = nameof(PasswordsNotMatching);
        public const string BadOldPassword = nameof(BadOldPassword);
    }
}