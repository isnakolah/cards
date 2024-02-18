using System.ComponentModel;

namespace Cards.Domain.Entities;

public enum CardStatus
{
    [Description("To Do")] ToDo,
    [Description("In Progress")] InProgress,
    [Description("Done")] Done
}