namespace MailerManager.Core.Actions;

[AttributeUsage(AttributeTargets.Class)]
public class ActionAttribute(string actionName) : Attribute
{
    public string ActionName { get; set; } = actionName;
}