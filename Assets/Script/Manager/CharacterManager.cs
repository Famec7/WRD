using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    protected override void Init()
    {
        ;
    }
    
    public enum CharacterType
    {
        Pet1,
        Pet2,
        Pet3,
        Pet4,
        Player,
        Pet5,
        Pet6,
        Pet7,
        Pet8,
        Count,
    }
    
    [SerializeField]
    private CharacterController[] _characters;
    
    public int CharacterCount => _characters.Length;
    
    public CharacterController GetCharacter(int index)
    {
        return _characters[index];
    }
    
    public CharacterController GetCharacter(CharacterType type)
    {
        return _characters[(int) type];
    }
}