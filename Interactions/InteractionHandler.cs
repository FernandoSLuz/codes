using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using CrimeTrip.Audio;
using JetBrains.Annotations;
using System.Linq;
using System;

namespace CrimeTrip.Interactions {
    public class InteractionHandler : MonoBehaviour {
        public int interactionIndex;
        [NonSerialized]
        public InteractionsController interactionsController;
        public Interaction interaction = new Interaction();

        public void Setup(InteractionsController newInteractionsController) {
            interactionsController = newInteractionsController;
            interaction.interactionHandler = this;
            interaction.interactionState = InteractionStates.locked;
            interaction.RegisterConditions();
            //interaction.ConditionMet();
        }
    }
    [System.Serializable]
    public class Interactables{

        //ActionTypes
        public string interactableDescription;
        [Header("What should we do after triggering?")]
        [AllowNesting]
        public InteractionTypes interactionType;

        //////////////////////////////////////////////// 
        [ShowIf("interactionType", InteractionTypes.HandleVoiceOver)]
        [AllowNesting]
        public AudioHandler audioHandler;
        [ShowIf("interactionType", InteractionTypes.HandleVoiceOver)]
        [AllowNesting]
        public bool stopAudio;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.PlayAnimation)]
        [AllowNesting]
        public AnimatorHandler animatorHandler;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.PlaySFX)]
        [AllowNesting]
        public SFXHandler sFXHandler;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.PlayBGM)]
        [AllowNesting]
        public BGMHandler bGMHandler;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.PlayTimeline)]
        [AllowNesting]
        public TimelineHandler timelineHandler;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.EnableGameObject)]
        [AllowNesting]
        public GameObject objToHandle;
        [ShowIf("interactionType", InteractionTypes.EnableGameObject)]
        [AllowNesting]
        public ActionTypes objAction;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.EnableGlitchEffect)]
        [AllowNesting]
        public GlitchEffectHandler glitchEffectHandler;
        [ShowIf("interactionType", InteractionTypes.EnableGlitchEffect)]
        [AllowNesting]
        public ActionTypes glitchAction;
        [ShowIf("interactionType", InteractionTypes.EnableGlitchEffect)]
        [AllowNesting]
        public float glitchActivationDelay;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.ShowTextHint)]
        [AllowNesting]
        public HintHandler hintHandler;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.EnableTrigger)]
        [AllowNesting]
        public BoxCollider triggerToHandle;
        [ShowIf("interactionType", InteractionTypes.EnableTrigger)]
        [AllowNesting]
        public ActionTypes triggerAction;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.HandleOtherRules)]
        [AllowNesting]
        public InteractionHandler targetInteractionHandler;
        [ShowIf("interactionType", InteractionTypes.HandleOtherRules)]
        [AllowNesting]
        public ActionTypes interactionHandlerAction;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.HandleUI)]
        [AllowNesting]
        public UiTypes UiType;
        [ShowIf("interactionType", InteractionTypes.HandleUI)]
        [AllowNesting]
        public bool enableUI = true;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.SetRespawn)]
        [AllowNesting]
        public GameObject respawn;

        ////////////////////////////////////////////////
        [ShowIf("interactionType", InteractionTypes.InvestigationBoard)]
        [AllowNesting]
        public BoardController boardController;
        [ShowIf("interactionType", InteractionTypes.InvestigationBoard)]
        [AllowNesting]
        public InvestigationBoardActions investigationBoardAction;        
        [ShowIf("investigationBoardAction", InvestigationBoardActions.enableInteractivity)]
        [AllowNesting]
        public bool investBoardEnable;

		////////////////////////////////////////////////
		[ShowIf("interactionType", InteractionTypes.SetSafeAreas)]
		[AllowNesting]
		public EligibleAreas eligibleAreas;

		////////////////////////////////////////////////
		[ShowIf("interactionType", InteractionTypes.SaveGamePoint)]
		[AllowNesting]
		public int loadPointIndex = 0;

		////////////////////////////////////////////////
		[ShowIf("interactionType", InteractionTypes.MoveObjectToNewPlace)]
		[AllowNesting]
		public GameObject objToMove;
		[ShowIf("interactionType", InteractionTypes.MoveObjectToNewPlace)]
		[AllowNesting]
		public Transform objectDestination;

		////////////////////////////////////////////////
		[ShowIf("interactionType", InteractionTypes.HandleDoor)]
		[AllowNesting]
		public GameDoorsList gameDoorReference;

		[ShowIf("gameDoorReference", GameDoorsList.CustomDoor)]
		[AllowNesting]
		public DoorHandler doorHandler;

        [ShowIf("interactionType", InteractionTypes.HandleDoor)]
        [AllowNesting]
        public DoorAction doorAction;

        [ShowIf("interactionType", InteractionTypes.HandleDoor)]
        [AllowNesting]
        public DoorLockAction doorLockAction;

    }
    [System.Serializable]
    public class Interaction{
        public string interactionDescription;
        [System.NonSerialized]
        public InteractionHandler interactionHandler;
        [System.NonSerialized]
        public UnityEvent conditionTrigger = new UnityEvent();
        [Header("Do we have any previous interaction" + "\n" + "that should be played before activating this condition?")]
        public List<InteractionHandler> InteractionsConditions = new List<InteractionHandler>();
        [Header("How should this interaction be triggered?")]
        public TriggerTypes triggerType;

        [ShowIf("triggerType", TriggerTypes.AfterASetTime)]
        [AllowNesting]
        public float startDelayTime;

        [ShowIf("triggerType", TriggerTypes.AfterCollisionWithObject)]
        [AllowNesting]
        public CollisionHandlers collisionHandlers;

        [ShowIf("triggerType", TriggerTypes.AfterInteractionWithObject)]
        [AllowNesting]
        public FingerInteractionsHandlers fingerInteractionsHandlers;

        [ShowIf("triggerType", TriggerTypes.AfterInteractionWithButton)]
        [AllowNesting]
        public BtnTypes btnType;

		[ShowIf("triggerType", TriggerTypes.LoadPoint)]
		[AllowNesting]
		public int loadPointIndex = 0;

		[ShowIf("triggerType", TriggerTypes.LoadPointRespawn)]
		[AllowNesting]
		public int loadPointRespawnIndex = 0;

        [ShowIf("triggerType", TriggerTypes.Failure)]
        [AllowNesting]
        public FailureHandler failureHandler;

        [ShowIf("triggerType", TriggerTypes.DoorOpened)]
        [AllowNesting]
        public DoorHandler doorHandler;

        [Header("Put here the list of actions when activated")]
        public List<Interactables> interactables = new List<Interactables>();

        [Header("Current state?")]
        public InteractionStates interactionState = InteractionStates.locked;

        public bool loop;

        public void RegisterConditions() {
            foreach (var interactionCondition in InteractionsConditions) {
                    if(interactionCondition != null) interactionCondition.interaction.conditionTrigger.AddListener(ConditionMet);
            }
            switch (triggerType) {
                case TriggerTypes.AfterCollisionWithObject:
                    try{
                        foreach(var collisionHandler in collisionHandlers.list){
                            collisionHandler.RegisteredInteractions.Add(interactionHandler);
                        }
                    } catch(System.Exception e){
                        //Debug.LogError(interactionHandler.gameObject.name);
                    }
                    foreach (var collisionHandler in collisionHandlers.list) {
                        collisionHandler.triggerEvent.AddListener(OnColliderTriggerEnter);
                    }
                    break;
                case TriggerTypes.AfterInteractionWithObject:
                    foreach (var fingerInteractionsHandlers in fingerInteractionsHandlers.list)
                    {
                        fingerInteractionsHandlers.RegisteredInteractions.Add(interactionHandler);
                        fingerInteractionsHandlers.triggerEvent.AddListener(OnInteractionEnter);
                    }
                    break;
                case TriggerTypes.AfterInteractionWithButton:
                    ButtonClickHandler buttonClickHandler;
                    switch (btnType) {
                        case BtnTypes.NextButton:
                            buttonClickHandler = UIManager.Instance.headerUIManager.nextButton.GetComponent<ButtonClickHandler>();
                            break;
                        default:
                            buttonClickHandler = UIManager.Instance.headerUIManager.skipButton.GetComponent<ButtonClickHandler>();
                            break;
                    }
                    buttonClickHandler.RegisteredInteractions.Add(interactionHandler);
                    buttonClickHandler.triggerEvent.AddListener(OnButtonClickEnter);
                    break;
				case TriggerTypes.AfterPaymentGateIsConfirmed:
					UIManager.Instance.headerUIManager.paymentGateHandler.RegisteredInteractions.Add(interactionHandler);
					UIManager.Instance.headerUIManager.paymentGateHandler.triggerEvent.AddListener(OnPaymentGateConfirmed);
					break;
				case TriggerTypes.LoadSavedGames:
                    LoadSavedGame(true);
                    ARFoundationController.Instance.releasedEvent.AddListener(()=> { LoadSavedGame(false); });
					break;
                case TriggerTypes.Failure:
                    failureHandler.RegisteredInteractions.Add(interactionHandler);
                    failureHandler.triggerEvent.AddListener(OnPlayerDeath);
                    break;
                case TriggerTypes.DoorOpened:
                    doorHandler.RegisteredInteractions.Add(interactionHandler);
                    doorHandler.triggerEvent.AddListener(OnDoorOpened);
                    break;
			}
        }
        public void LoadSavedGame(bool onlySetRespawn) {
            int loadIndex = 0;
            if (PlayerPrefs.HasKey(GameManager.Instance.ExpWidget + "_SavePoint")) {
                loadIndex = PlayerPrefs.GetInt(GameManager.Instance.ExpWidget + "_SavePoint");
			}
            if (onlySetRespawn)
            {
				Debug.LogError("PLACING RESPAWN");
                InteractionHandler respawnInteractionHandler = interactionHandler.interactionsController.interactionsOrder.Single(x => x.interactionHandler.interaction.triggerType == TriggerTypes.LoadPointRespawn
                                                                                                                            && x.interactionHandler.interaction.loadPointRespawnIndex == loadIndex).interactionHandler;
				respawnInteractionHandler.StartCoroutine(respawnInteractionHandler.interaction.InteractionSetupRoutine(true));
			}
            else {
				Debug.LogError("STARTING GAME");
				InteractionHandler gameStartInteractionHandler = interactionHandler.interactionsController.interactionsOrder.Single(x => x.interactionHandler.interaction.triggerType == TriggerTypes.LoadPoint
																												&& x.interactionHandler.interaction.loadPointIndex == loadIndex).interactionHandler;
				gameStartInteractionHandler.StartCoroutine(gameStartInteractionHandler.interaction.InteractionSetupRoutine(true));
			}



		}
        public void MarkInteractionAsComplete() {
            conditionTrigger.Invoke();
        }
        public void ConditionMet() {
            bool allConditionsMet = true;
            foreach (var interactionCondition in InteractionsConditions) {
                if (interactionCondition != null){
                    if (interactionCondition.interaction.interactionState != InteractionStates.triggered) {
                        allConditionsMet = false;
                        break;
                    }
                }
            }
            if (allConditionsMet && interactionState != InteractionStates.triggered && triggerType != TriggerTypes.AlwaysLocked) {
                interactionHandler.StartCoroutine(InteractionSetupRoutine());
            }
        }
        //HERE IS THE METHODS THAT WILL EXECUTE THE INTERACTIVITY
        public IEnumerator InteractionSetupRoutine(bool forcePlay = false) {
            //Debug.LogError(interactionHandler.name);
            if (triggerType == TriggerTypes.AfterASetTime) {
                interactionState = InteractionStates.triggering;
                float timer = 0;
                while (timer < startDelayTime) {
                    timer += Time.deltaTime * 1.0f;
                    yield return null;
                }
                yield return interactionHandler.StartCoroutine(HandleInteractivity());
            } else {
                interactionState = InteractionStates.unlocked;
                //Debug.LogError(interactionHandler);
                if (forcePlay) {
                    while (interactionHandler == null) {
						yield return null;
					}
					yield return interactionHandler.StartCoroutine(HandleInteractivity());
				}
			}
        }
        public void OnInteractionEnter()
        {
            if (triggerType == TriggerTypes.AfterInteractionWithObject && interactionState == InteractionStates.unlocked)
            {
                interactionHandler.StartCoroutine(HandleInteractivity());
            }
        }
        public void OnColliderTriggerEnter()
        {
            if (triggerType == TriggerTypes.AfterCollisionWithObject && interactionState == InteractionStates.unlocked)
            {
                interactionHandler.StartCoroutine(HandleInteractivity());
            }
        }
        public void OnButtonClickEnter()
        {
            if (triggerType == TriggerTypes.AfterInteractionWithButton && interactionState == InteractionStates.unlocked)
            {
                interactionHandler.StartCoroutine(HandleInteractivity());
            }
        }
        public void OnPaymentGateConfirmed()
        {
            if (triggerType == TriggerTypes.AfterPaymentGateIsConfirmed && interactionState == InteractionStates.unlocked)
            {
                interactionHandler.StartCoroutine(HandleInteractivity());
            }
        }
        public void OnPlayerDeath()
        {
            if (triggerType == TriggerTypes.Failure && interactionState == InteractionStates.unlocked)
            {
                interactionHandler.StartCoroutine(HandleInteractivity());
            }
        }

        public void OnDoorOpened()
        {
            if (triggerType == TriggerTypes.DoorOpened && interactionState == InteractionStates.unlocked)
            {
                interactionHandler.StartCoroutine(HandleInteractivity());
            }
        }

        public void CancelInteraction(bool reset = false){
            interactionHandler.StopAllCoroutines();
            foreach (var interactivity in interactables) {
                if (interactivity.interactionType == InteractionTypes.HandleVoiceOver) {
                    if(interactivity.audioHandler != null){
                        interactivity.audioHandler.StopAllCoroutines();
                        interactivity.audioHandler.StartCoroutine(interactivity.audioHandler.Stop(interactivity.audioHandler, true));
                    }else{
                        Debug.LogError("MISSING REFERENCE ON AUDIOHANDLER AT " + interactionHandler.gameObject.name);
                    }

                } else if (interactivity.interactionType == InteractionTypes.ShowTextHint) {
                    if (interactivity.hintHandler != null) {
                        interactivity.hintHandler.StopAllCoroutines();
                        interactivity.hintHandler.StartCoroutine(interactivity.hintHandler.Stop());
                    } else {
                        Debug.LogError("MISSING REFERENCE ON HINTHANDLER AT " + interactionHandler.gameObject.name);
                    }
                }else{
                    if (interactivity.interactionType != InteractionTypes.HandleOtherRules) {
                        HandleInstantInteractables(interactivity, true);
                    }
                }
            }
            if(!reset){
                interactionState = InteractionStates.triggered;
            } else{
                interactionState = InteractionStates.locked;
            }
			MarkInteractionAsComplete();
		}
        //AFTER THE EXECUTION TRIGGER IS ACTIVATED, ME MUS ENABLE/HANDLE THE SELECTED INTERACTIVITY
        public IEnumerator HandleInteractivity() {
            interactionHandler.interaction.interactionState = InteractionStates.triggering;
            InteractionHandler[] interactionHandlers = GameObject.FindObjectsOfType<InteractionHandler>();
            Debug.Log("EXECUTING INTERACTIVITIES ON " + interactionHandler.name);
            foreach (var interactable in interactables) {
                //here we will run interactions that won't contain delayed time, like enabling an object
                HandleInstantInteractables(interactable);
            }
            foreach (var interactable in interactables) {
                //here we will run interactions that possibily will contai delayed time, like audio and animations
                if(interactable.interactionType == InteractionTypes.HandleVoiceOver || interactable.interactionType == InteractionTypes.EnableGlitchEffect){
                    yield return interactionHandler.StartCoroutine(HandleDelayedInteractables(interactable));
                }
            }
            if(loop){
                interactionState = InteractionStates.unlocked;
            } else{
                interactionState = InteractionStates.triggered;
            }
            
            yield return null;
            MarkInteractionAsComplete();
        }
        public void HandleInstantInteractables(Interactables interactable, bool skip = false) {
            switch (interactable.interactionType) {
                case InteractionTypes.EnableGameObject:
                    if (interactable.objToHandle != null) {
                        if (interactable.objAction == ActionTypes.Enable) interactable.objToHandle.SetActive(true);
                        else interactable.objToHandle.SetActive(false);
                    } else {
                        Debug.LogError("MISSING REFERENCE ON OBJECTTOHANDLE ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.EnableTrigger:
                    if (interactable.triggerToHandle != null) {
                        if (interactable.triggerAction == ActionTypes.Enable) interactable.triggerToHandle.enabled = true;
                        else interactable.triggerToHandle.enabled = false;
                    } else {
                        Debug.LogError("MISSING REFERENCE ON TRIGGERTOHANDLER ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.ShowTextHint:
                    if (interactable.hintHandler != null) {
                        if (!skip) {
                            interactable.hintHandler.StartCoroutine(interactable.hintHandler.Play(interactionHandler));
                        }
                    } else {
                        Debug.LogError("MISSING REFERENCE ON HINTHANDLER ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.PlayAnimation:
                    if (interactable.animatorHandler != null) {
                        if (loop) interactable.animatorHandler.loop = true;
                        interactable.animatorHandler.PlayAnimation(skip);
                    } else {
                        Debug.LogError("MISSING REFERENCE ON ANIMATOR ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.PlayTimeline:
                    if (interactable.timelineHandler != null) {
                        interactable.timelineHandler.PlayAnimation(skip);
                    } else {
                        Debug.LogError("MISSING REFERENCE ON TIMELINE ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.HandleOtherRules:

                    if (interactable.targetInteractionHandler != null) {
                        interactable.targetInteractionHandler.StopAllCoroutines();
                        if (interactable.interactionHandlerAction == ActionTypes.Disable) {
                            interactable.targetInteractionHandler.interaction.CancelInteraction();
                        } else if (interactable.interactionHandlerAction == ActionTypes.Enable || interactable.interactionHandlerAction == ActionTypes.EnableAndForcePlay)
						{
                            bool forcePlay = interactable.interactionHandlerAction == ActionTypes.EnableAndForcePlay ? true: false;
							interactable.targetInteractionHandler.StartCoroutine(interactable.targetInteractionHandler.interaction.InteractionSetupRoutine(forcePlay));
                        }else{
                            interactable.targetInteractionHandler.interaction.CancelInteraction(true);
                        }
                    } else {
                        Debug.LogError("MISSING REFERENCE ON TARGETINTERACTIONHANDLER ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.EnableGlitchEffect:

                    if (interactable.glitchEffectHandler != null)
                    {
                        if (interactable.glitchAction == ActionTypes.Enable)
                        {
                            interactable.glitchEffectHandler.glitchActivationCoroutine = interactable.glitchEffectHandler.StartCoroutine(interactable.glitchEffectHandler.ActivateGlitchEffect(interactable.glitchActivationDelay));
                        }
                        else
                        {
                            interactable.glitchEffectHandler.DeactivateGlitchEffect();
                        }
                    }
                    else
                    {
                        Debug.LogError("MISSING REFERENCE FOR GLITCHEFFECTHANDLER ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.InvestigationBoard:

                    if(interactable.boardController != null)
                    {
                        if(interactable.investigationBoardAction == InvestigationBoardActions.flipBoard)
                        {
                            interactable.boardController.investBoardFlipHandler.BeginBoardFlip();
                        }
                        else if(interactable.investigationBoardAction == InvestigationBoardActions.enableInteractivity)
                        {
                            interactable.boardController.investBoardFlipHandler.enableInteractivity(interactable.investBoardEnable);
                        }
                    }
                    else
                    {
                        Debug.LogError("MISSING REFERENCE FOR BOARDCONTROLLER ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.PlaySFX:

                    if (interactable.sFXHandler != null) {
                        interactable.sFXHandler.PlaySFX();
                    } else {
                        Debug.LogError("MISSING REFERENCE FOR SFXHandler ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.PlayBGM:
                    if (interactable.bGMHandler != null)
                    {
                        interactable.bGMHandler.StartCoroutine(interactable.bGMHandler.RequestBGMMusic());
                    }
                    else
                    {
                        Debug.LogError("MISSING REFERENCE FOR BGMHandler ON " + interactionHandler.gameObject.name);
                    }
                    break;
                case InteractionTypes.HandleUI:
                    switch (interactable.UiType)
                    {
                        case UiTypes.HeightSlider:
                            UIManager.Instance.headerUIManager.heightSlider.gameObject.SetActive(interactable.enableUI);
                            break;
                        case UiTypes.Joysticks:
							UIManager.Instance.joystickHandler.HandleJoysticks(interactable.enableUI);
                            break;
                        case UiTypes.SkipButton:
                            UIManager.Instance.headerUIManager.skipButton.gameObject.SetActive(interactable.enableUI);
                            break;
						case UiTypes.NextButton:
							UIManager.Instance.headerUIManager.nextButton.gameObject.SetActive(interactable.enableUI);
							break;
						case UiTypes.videoPlayer:
							UIManager.Instance.headerUIManager.videoPlayer.SetActive(interactable.enableUI);
							break;
						case UiTypes.recalibrationButton:
							UIManager.Instance.headerUIManager.recalibrationButton.SetActive(interactable.enableUI);
							break;
						case UiTypes.reticle:
							UIManager.Instance.headerUIManager.reticle.SetActive(interactable.enableUI);
							break;
					}
                    break;
                case InteractionTypes.SetRespawn:
                    CameraTools.RespawnCameraController.instance.DefineNewRespawn(interactable.respawn);
                    break;
                case InteractionTypes.Respawn:
                    CameraTools.RespawnCameraController.instance.Respawn();
                    break;
				case InteractionTypes.PaymentGate:
					UIManager.Instance.headerUIManager.paymentGateHandler.OpenPaymentGate();
					break;
				case InteractionTypes.SetSafeAreas:
					OOBController.instance.SetSafeAreas(interactable.eligibleAreas);
					break;
				case InteractionTypes.SaveGamePoint:
					PlayerPrefs.SetInt(GameManager.Instance.ExpWidget + "_SavePoint", interactable.loadPointIndex);
					Debug.LogError("--------- GAME SAVED AT STATE " + interactable.loadPointIndex + " ---------");
					break;
				case InteractionTypes.MoveObjectToNewPlace:
					interactable.objToMove.transform.position = interactable.objectDestination.position;
					interactable.objToMove.transform.rotation = interactable.objectDestination.rotation;
					break;
                case InteractionTypes.HandleDoor:
                    if (interactable.gameDoorReference == GameDoorsList.CustomDoor)
                    {
						DoorController.instance.HandleDoor(interactable.doorHandler, interactable.doorLockAction, interactable.doorAction);
					}
                    else {
						DoorController.instance.HandleDoor(interactable.gameDoorReference, interactable.doorLockAction, interactable.doorAction);
					}
                    break;
			}
        }
        public IEnumerator HandleDelayedInteractables(Interactables interactable) {
            switch (interactable.interactionType) {
                case InteractionTypes.HandleVoiceOver:
                    if (interactable.audioHandler != null) {
                        switch (interactable.audioHandler.audioTriggeringStatusCondition) {
                            case AudioTriggeringStatusConditions.OnStart:
                                interactable.audioHandler.StartCoroutine(interactable.audioHandler.Play(interactable.stopAudio));
                                break;
                            case AudioTriggeringStatusConditions.OnFinish:
                                yield return interactable.audioHandler.StartCoroutine(interactable.audioHandler.Play(interactable.stopAudio));
                                break;
                        }
                    } else {
                        Debug.LogError("MISSING REFERENCE ON AUDIOHANDLER ON " + interactionHandler.gameObject.name);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    [System.Serializable]
    public class CollisionHandlers {
        public List<CollisionHandler> list = new List<CollisionHandler>();
    }
    [System.Serializable]
    public class FingerInteractionsHandlers {
        public List<FingerInteractionsHandler> list = new List<FingerInteractionsHandler>();
    }
    [System.Serializable]
    public enum TriggeringStatusConditions {
        OnStart,
        AfterASetTime,
        OnFinish
    }
    [System.Serializable]
    public enum AudioTriggeringStatusConditions {
        OnFinish,
        OnStart
    }
    [System.Serializable]
    public enum AudioLabels{
        Instruction,
        Contextual,
        Suggestion
    }
    [System.Serializable]
    public enum TriggerTypes {
        AfterCollisionWithObject,
        AfterASetTime,
        AfterInteractionWithObject,
        AlwaysLocked,
        AfterInteractionWithButton,
        AfterPaymentGateIsConfirmed,
        LoadSavedGames,
        LoadPoint,
        LoadPointRespawn,
        Failure,
        DoorOpened
    }
    [System.Serializable]
    public enum InteractionTypes
    {
        HandleVoiceOver,
        PlayAnimation,
        EnableGameObject,
        ShowTextHint,
        EnableTrigger,
        HandleOtherRules,
        PlayTimeline,
        EnableGlitchEffect,
        InvestigationBoard,
        PlaySFX,
        PlayBGM,
        GrabObject,
        HoldObject,
        DropObject,
        HandleUI,
        SetRespawn,
        Respawn,
        PaymentGate,
        SetSafeAreas,
        SaveGamePoint,
        MoveObjectToNewPlace,
        HandleDoor
    }
    [System.Serializable]
    public enum UiTypes
    {
        Joysticks,
        HeightSlider,
        SkipButton,
        NextButton,
        videoPlayer,
        recalibrationButton,
        reticle
    }
    [System.Serializable]
    public enum BtnTypes
    {
        SkipButton,
        NextButton
    }

    [System.Serializable]
    public enum ActionTypes {
        Enable,
        Disable,
        Reset,
        EnableAndForcePlay
    }
    [System.Serializable]
    public enum InteractionStates {
        locked,
        unlocked,
        triggering,
        triggered
    }
    [System.Serializable]
    public enum InvestigationBoardActions
    {
        flipBoard,
        enableInteractivity
    }
}