using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using rnd = UnityEngine.Random;

public class NeeDeezNuts : MonoBehaviour{
    public new KMAudio Audio;
    public KMBombInfo bomb;
    public KMNeedyModule module;
    public KMSelectable[] MasherTheBottun;
    public TextMesh[] IWillBombTheAtlanticOcean;

    private bool active;
    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved;
    int Penis = 0;
    bool[] Wheufhahekfhe = {false,false,false,false};

    void Awake(){
        moduleId = moduleIdCounter++;
        module.OnNeedyActivation += OnNeedyActivation;
        module.OnNeedyDeactivation += OnNeedyDeactivation;
        module.OnTimerExpired += OnTimerExpired;
        foreach (KMSelectable Dick in MasherTheBottun) {
            Dick.OnInteract += delegate () { DickPress(Dick); return false; };
        }
    }

    void Start(){
        module.SetResetDelayTime(30f,50f);
    }
    void DickPress(KMSelectable Dick){
      Dick.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Dick.transform);
      for (int i = 0; i < 4; i++) {
        if (Dick == MasherTheBottun[i] && Wheufhahekfhe[i] == true) {
          Debug.LogFormat("[NeeDeez Nuts #{0}] Deez nuts!", moduleId);
          Audio.PlaySoundAtTransform("DeezNuts", transform);
          OnNeedyDeactivation();
          module.OnPass();
          Dick.AddInteractionPunch();
          Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Dick.transform);
          for (int j = 0; j < 4; j++) {
            Wheufhahekfhe[j] = false;
            IWillBombTheAtlanticOcean[j].text = "Got Eeem!";
          }
        }
        else if(Dick == MasherTheBottun[i] && Wheufhahekfhe[i] == false) {
          Debug.LogFormat("[NeeDeez Nuts #{0}] Got Eeem!", moduleId);
          Audio.PlaySoundAtTransform("GOTEEE", transform);
          module.OnStrike();
          Dick.AddInteractionPunch();
          Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Dick.transform);
        }
      }
    }
    protected void OnNeedyActivation(){
        active = true;
        Penis = UnityEngine.Random.Range(0,4);
        Wheufhahekfhe[Penis] = true;
        IWillBombTheAtlanticOcean[Penis].text = "Deez Nuts!";
        for (int i = 0; i < 4; i++) {
          if (i != Penis) {
            IWillBombTheAtlanticOcean[i].text = "Got Eeem!";
          }
        }
    }

    protected void OnNeedyDeactivation(){
        active = false;
    }

    protected void OnTimerExpired(){
        if (active){
            module.OnStrike();
            OnNeedyDeactivation();
        }
    }
    //I add the twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press TL/TR/BL/BR to press the corresponding button.";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command){
      command = command.Trim();
      string[] parameters = command.Split(' ');
      if (parameters.Length > 2) {
        yield return null;
        yield return "sendtochaterror Too many words";
        yield break;
      }
      else if (parameters.Length == 2) {
        yield return null;
        if (parameters[0].ToUpper() == "PRESS") {
          if (parameters[1].ToUpper() == "TL") {
            MasherTheBottun[0].OnInteract();
          }
          else if (parameters[1].ToUpper() == "TR") {
            MasherTheBottun[1].OnInteract();
          }
          else if (parameters[1].ToUpper() == "BL") {
            MasherTheBottun[3].OnInteract();
          }
          else if (parameters[1].ToUpper() == "BR") {
            MasherTheBottun[2].OnInteract();
          }
          else {
            yield return "sendtochaterror Invalid command.";
            yield break;
          }
        }
        else {
          yield return null;
          yield return "sendtochaterror Invalid command.";
          yield break;
        }
      }
      if (parameters.Length < 2) {
        yield return null;
        yield return "sendtochaterror Too little words";
        yield break;
      }
    }
}
