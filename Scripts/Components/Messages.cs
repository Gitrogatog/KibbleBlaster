namespace MyECS.Messages;

public readonly record struct PlaySoundMessage(SoundName Name);
public readonly record struct DisplayDialogMessage(int ID);