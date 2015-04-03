using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ParameterGUIInterface : MonoBehaviour
{
    // public fields
    public bool MakeDuplicates = true; // if true: will make NumberOfParameters*2 parameters, where the last NumberOfParameters are duplicates
    public GameObject Player;
    public bool DrawDebugMenu = true;
    public GUIStyle DebugGUIStyle;
    public GUISkin HorizontalSliderSkin;

    public Text GameNumberText;

    // internal links
    private Player _player;
    private PostDataOnline _myPostDataOnline;
    private ParameterManager _paramateManager;
    private TweakableParameters _parameters;
    

    // internal fields
    private bool _showExtendedGUI = false;
    private Vector2 scrollViewVector = Vector2.zero;


    // strings
    private string name = "Write your name here";
    private string feeling = "Describe this game feeling here";

    void Start()
    {
        _player = Player.GetComponent<Player>();
        _myPostDataOnline = GetComponent<PostDataOnline>();

        if (_player == null)
            Debug.Log("Error - ParameterGUIInterface needs link to Player script!");

        if (_myPostDataOnline == null)
            Debug.Log("Error - ParameterGUIInterface needs link to PostDataOnline script!");

        _parameters = _player.MyTweakableParameters;

        
    }

    void Update()
    {
        _parameters = _player.MyTweakableParameters;


        /*if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Creating " + ParameterManager.Instance.NumberOfParameters + " new paramaters");
            ParameterManager.Instance.MakeParameters(ParameterManager.Instance.NumberOfParameters, false);
        }*/

        /*if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (ParameterManager.Instance.Index + 1 < ParameterManager.Instance.MyParameters.Count)
                ParameterManager.Instance.Index++;
            else
                ParameterManager.Instance.Index = 0;

            _player.ChangeParameters();

            _player.Restart();

        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (ParameterManager.Instance.Index - 1 >= 0)
                ParameterManager.Instance.Index--;
            else
                ParameterManager.Instance.Index = ParameterManager.Instance.MyParameters.Count-1;

            _player.ChangeParameters();

        }*/

        // show debug interface
        //if (Input.GetKeyDown(KeyCode.Y))
          //  DrawDebugMenu = !DrawDebugMenu;


        GameNumberText.text = string.Format("Round: {0} / {1}", (ParameterManager.Instance.Index + 1),
            ParameterManager.Instance.MyParameters.Count);
    }

    

    void OnGUI()
    {
        // send data
        /*name = GUI.TextField(new Rect(Screen.width - (Screen.width * 0.3f), Screen.height - (Screen.height * 0.15f), 200, 20), name);
        feeling = GUI.TextField(new Rect(Screen.width - (Screen.width * 0.3f), Screen.height - (Screen.height * 0.08f), 200, 20), feeling);

        if (GUI.Button(new Rect(Screen.width - (Screen.width * 0.6f), Screen.height - (Screen.height * 0.15f), 130, 20), "Send data"))
        {
            _myPostDataOnline.PostData(Demographics.Instance.YourName, Demographics.Instance.ToStringDatabaseFormat(), _player.MyTweakableParameters.MyRating.ToStringDatabaseFormat(), _player.MyTweakableParameters.ToStringDatabaseFormat(), StateManager.Instance.AverageFps);
        }

        if (GUI.Button(new Rect(Screen.width - (Screen.width * 0.6f), Screen.height - (Screen.height * 0.08f), 130, 20), "View data"))
        {
            Application.OpenURL("http://tunnelvisiongames.com/unityserver/displayfeeling.php");
        }*/



        if (!DrawDebugMenu)
            return;


        // show parameters
        GUILayout.BeginArea(new Rect(Screen.width, Screen.height, Screen.width, Screen.height*0.9f));
        scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
        GUILayout.BeginVertical();
            GUILayout.Label("TWEAKABLE PARAMATERS:\t\n", DebugGUIStyle);

            GUILayout.Label("Gravity: " + _parameters.Gravity.y, DebugGUIStyle);
            _parameters.Gravity.y = GUILayout.HorizontalSlider(_parameters.Gravity.y, TweakableParameters.GravityRange.x,
                TweakableParameters.GravityRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("Jump Power: " + _parameters.JumpPower, DebugGUIStyle);
            _parameters.JumpPower = GUILayout.HorizontalSlider(_parameters.JumpPower, TweakableParameters.JumpPowerRange.x,
                TweakableParameters.JumpPowerRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            //GUILayout.Label("UseAirFriction: " + _parameters.UseAirFriction, DebugGUIStyle);
            //_parameters.UseAirFriction = GUILayout.Toggle(_parameters.UseAirFriction, "Use AirFriction");

            GUILayout.Label("AirFrictionHorizontalPercent: " + _parameters.AirFrictionHorizontalPercentage, DebugGUIStyle);
            _parameters.AirFrictionHorizontalPercentage = GUILayout.HorizontalSlider(_parameters.AirFrictionHorizontalPercentage, TweakableParameters.AirFrictionHorizontalPercentageRange.x,
                TweakableParameters.AirFrictionHorizontalPercentageRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("TerminalVelocity: " + _parameters.TerminalVelocity, DebugGUIStyle);
            _parameters.TerminalVelocity = GUILayout.HorizontalSlider(_parameters.TerminalVelocity, TweakableParameters.TerminalVelocityRange.x,
                TweakableParameters.TerminalVelocityRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("GhostJumpTime: " + _parameters.GhostJumpTime, DebugGUIStyle);
            _parameters.GhostJumpTime = GUILayout.HorizontalSlider(_parameters.GhostJumpTime, TweakableParameters.GhostJumpTimeRange.x,
                TweakableParameters.GhostJumpTimeRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("MinimumJumpHeight: " + _parameters.MinimumJumpHeight, DebugGUIStyle);
            _parameters.MinimumJumpHeight = GUILayout.HorizontalSlider(_parameters.MinimumJumpHeight, TweakableParameters.MinimumJumpHeightRange.x,
                TweakableParameters.MinimumJumpHeightRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("ReleaseEarlyJumpVelocity: " + _parameters.ReleaseEarlyJumpVelocity, DebugGUIStyle);
            _parameters.ReleaseEarlyJumpVelocity = GUILayout.HorizontalSlider(_parameters.ReleaseEarlyJumpVelocity, TweakableParameters.ReleaseEarlyJumpVelocityRange.x,
                TweakableParameters.ReleaseEarlyJumpVelocityRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("ApexGravityMultiplier: " + _parameters.ApexGravityMultiplier, DebugGUIStyle);
            _parameters.ApexGravityMultiplier = GUILayout.HorizontalSlider(_parameters.ApexGravityMultiplier, TweakableParameters.ApexGravityMultiplierRange.x,
                TweakableParameters.ApexGravityMultiplierRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("MaxVelocityX: " + _parameters.MaxVelocityX, DebugGUIStyle);
            _parameters.MaxVelocityX = GUILayout.HorizontalSlider(_parameters.MaxVelocityX, TweakableParameters.MaxVelocityXRange.x,
                TweakableParameters.MaxVelocityXRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            //GUILayout.Label("UseGroundFriction: " + _parameters.UseGroundFriction, DebugGUIStyle);
            //_parameters.UseGroundFriction = GUILayout.Toggle(_parameters.UseGroundFriction, "UseGroundFriction");

            //GUILayout.Label("GroundFrictionPercent: " + _parameters.GroundFrictionPercentage, DebugGUIStyle);
            //_parameters.GroundFrictionPercentage = GUILayout.HorizontalSlider(_parameters.GroundFrictionPercentage, TweakableParameters.GroundFrictionPercentageRange.x,
            //   TweakableParameters.GroundFrictionPercentageRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("AttackTime: " + _parameters.AttackTime, DebugGUIStyle);
            _parameters.AttackTime = GUILayout.HorizontalSlider(_parameters.AttackTime, TweakableParameters.AttackTimeRange.x,
                TweakableParameters.AttackTimeRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("ReleaseTime: " + _parameters.ReleaseTime, DebugGUIStyle);
            _parameters.ReleaseTime = GUILayout.HorizontalSlider(_parameters.ReleaseTime, TweakableParameters.ReleaseTimeRange.x,
                TweakableParameters.ReleaseTimeRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            //GUILayout.Label("TurnAroundBoostPercent: " + _parameters.TurnAroundBoostPercent, DebugGUIStyle);
            //_parameters.TurnAroundBoostPercent = GUILayout.HorizontalSlider(_parameters.TurnAroundBoostPercent, TweakableParameters.TurnAroundBoostPercentRange.x,
            //    TweakableParameters.TurnAroundBoostPercentRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);

            GUILayout.Label("AnimationMaxSpeed: " + _parameters.AnimationMaxSpeed, DebugGUIStyle);
            _parameters.AnimationMaxSpeed = GUILayout.HorizontalSlider(_parameters.AnimationMaxSpeed, TweakableParameters.AnimationMaxSpeedRange.x,
                TweakableParameters.AnimationMaxSpeedRange.y, HorizontalSliderSkin.horizontalSlider, HorizontalSliderSkin.horizontalSliderThumb);


        GUILayout.EndVertical();
        GUI.EndScrollView();
        GUILayout.EndArea();




        GUI.Label(new Rect(Screen.width * 0.5f, 0, 180, 20), "Velocity: " + _player._velocity, DebugGUIStyle);
        GUI.Label(new Rect(Screen.width * 0.5f, 20, 180, 20), "State: " + _player._currentHorizontalMovementState, DebugGUIStyle);
        GUI.Label(new Rect(Screen.width * 0.5f, 40, 180, 20), "Index: " + (ParameterManager.Instance.Index+1) + " / " + ParameterManager.Instance.MyParameters.Count, DebugGUIStyle);
        GUI.Label(new Rect(Screen.width * 0.5f, 60, 180, 20), "Level: " + Application.loadedLevelName, DebugGUIStyle);
        GUI.Label(new Rect(Screen.width * 0.5f, 80, 180, 20), "GameState: " + Demographics.Instance.MyGameState, DebugGUIStyle);
        GUI.Label(new Rect(Screen.width * 0.5f, 100, 180, 20), "Grounded: " + _player._collisionState.IsGrounded, DebugGUIStyle);

    }
}
