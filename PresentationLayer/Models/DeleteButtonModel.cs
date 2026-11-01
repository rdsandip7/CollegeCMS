namespace PresentationLayer.Models;

/// <summary>Model for the shared confirm-then-POST delete button.</summary>
public record DeleteButtonModel(string Controller, string Id, string Name, string Action = "Delete");
