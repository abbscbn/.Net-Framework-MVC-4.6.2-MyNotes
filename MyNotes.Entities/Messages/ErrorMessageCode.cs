namespace MyNotes.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExists = 151,
        EmailAlreadyExists = 152,
        UserNotFound = 153,
        UserIsNotActive = 154,
        EmailOrPasswordWrong = 155,
        UserAlreadyActive = 156,
        UserCouldNotActive = 157,
        ActivationIdDoesNotExists = 158,
        UserCouldNotUpdated = 159,
        UserCouldNotDeleted = 160,
        CategoryNotFound = 161,
        CategoryAlreadyExists = 162,
        CategoryCouldNotUpdated = 163
    }
}
