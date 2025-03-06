using Godot;
namespace Assets.Godot;
[GlobalClass]
public partial class CharBodyResource : Resource
{
    [Export] public CharBodyGroup group;
    [Export] public PackedScene body;
}
