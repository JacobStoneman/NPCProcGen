using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class NPCController : MonoBehaviour
{
    [System.Serializable]
    public struct TraitAspect
    {
        public int id;
        public float negativeValue;
        public float positiveValue;
        public float overallAspectValue;
        public void SetValue()
        {
            overallAspectValue = positiveValue - negativeValue;
        }
        public void UpdateValue(float val)
        {
            overallAspectValue = val;
        }
    }
    [System.Serializable]
    public struct Trait
    {
        public float overallTraitValue;
        public TraitAspect aspect1;
        public TraitAspect aspect2;
        public TraitAspect aspect3;
        public TraitAspect aspect4;
        public void SetTraitOverallValue()
        {
            overallTraitValue = aspect1.overallAspectValue + aspect2.overallAspectValue + aspect3.overallAspectValue + aspect4.overallAspectValue;
            if (overallTraitValue != 0)
            {
                overallTraitValue /= 4;
            }
        }
    }
    [System.Serializable]
    public struct modification
    {
        public string aspectName;
        public float modifier;
    }
    [System.Serializable]
    public class BranchLifeEvent
    {
        public string eventText;
        public string nextTag;
        public int branchLevel;
        public List<modification> modifications = new List<modification>();
    }

    [System.Serializable]
    public class ChildhoodLifeEvent
    {
        public string eventText;
        public bool positive;
        public List<modification> modifications = new List<modification>();

    }
    public List<ChildhoodLifeEvent> childhoodLifeEvents = new List<ChildhoodLifeEvent>();

    [System.Serializable]
    public struct Need
    {
        public float value;
        public float decreaseVal;
        public int needMode;
    }

    [System.Serializable]
    public class LifeEvent
    {
        public string eventText;
        public int id;
        public string tag;
        public string nextTag;
        public List<BranchLifeEvent> backstoryBranches = new List<BranchLifeEvent>();
        public List<modification> modifications = new List<modification>();

    }
    public List<LifeEvent> lifeEvents = new List<LifeEvent>();
    public List<BranchLifeEvent> militaryBranch = new List<BranchLifeEvent>();
    public List<BranchLifeEvent> jobBranch = new List<BranchLifeEvent>();
    public List<BranchLifeEvent> adventureBranch = new List<BranchLifeEvent>();
    System.Random rnd;

    [System.Serializable]
    public class House
    {
        public enum HouseTrait
        {
            social,
            isolated,
            neutral
        }
        public GameObject bedPoint;
        public GameObject chairPoint;
        public GameObject houseObject;
        public HouseTrait houseTrait;
    }

    [System.Serializable]
    public class Lake
    {
        public GameObject centrePoint;
        public List<GameObject> fishingPoints;
    }

    [System.Serializable]
    public class Tavern
    {
        public List<GameObject> tavernPoints;
        public List<GameObject> tables;
    }

    [System.Serializable]
    public class WorkPoint
    {
        public GameObject point;
        public string description;
        public string title;
        public float exLow;
        public float exHigh;

        public float agLow;
        public float agHigh;

        public float conLow;
        public float conHigh;

        public float neuroLow;
        public float neuroHigh;

        public float openLow;
        public float openHigh;
    }

    [System.Serializable]
    public class NPC
    {
        public enum NPCState
        {
            DECIDING,
            ENERGIZE,
            WORKING,
            EATING,
            TALKING,
            RECREATION
        }
        public enum EatingState
        {
            EATATHOME,
            EATATTAVERN,
            STEALFOOD
        }
        public enum RecState
        {
            FISH,
            EXPLORE,
            HIDEATHOME,
            MARKET,
            TAVERN
        }
        public string name;
        int age;
        public float loner, joiner, quiet, talkative, passive, active, reserved, affectionate, suspicious, trusting, critical, lenient, ruthless, softHearted, irritable, goodNatured, negligent, conscientious, lazy, hardWorking, disorganized, wellOrganized, late, punctual, calm, worried, evenTempered, temperamental, comfortable, selfConscious, unemotional, emotional, downToEarth, imaginative, uncreative, creative, conventional, original, uncurious, curious;
        public Trait extroversion;
        public Trait agreeableness;
        public Trait conscientiousness;
        public Trait neuroticism;
        public Trait openness;
        public Need hunger;
        public Need social;
        public Need energy;
        public NPCState state;
        public EatingState eatState;
        public RecState recState;
        public string backstory;
        public string logText;
        public ChildhoodLifeEvent childhood;
        public List<LifeEvent> backstoryEvents = new List<LifeEvent>();
        public List<int> idList = new List<int>();
        public GameObject NPCObject;
        public GameObject camTarget;
        public WorkPoint workPoint;
        public House home;
        public AICharacterControl aiControl;
        //Weights////////////////
        public float socialWeight;
        public float hungerWeight;
        public float energyWeight;
        public float workWeight;
        public float recreationWeight;
        public float agreeablenessWeight;
        public float conversationWeight;
        //////////////////////////  
        public bool employed;
        bool talkInit;
        public float conversationLength;
        NPC talkTarget;
        NPC previousTalkTarget;
        int endTime;
        public string currentActionText;
        public void InitExtroversion()
        {
            extroversion.aspect1.id = 1;
            extroversion.aspect2.id = 2;
            extroversion.aspect3.id = 3;
            extroversion.aspect4.id = 4;
            extroversion.aspect1.negativeValue = loner;
            extroversion.aspect1.positiveValue = joiner;
            extroversion.aspect2.negativeValue = quiet;
            extroversion.aspect2.positiveValue = talkative;
            extroversion.aspect3.negativeValue = passive;
            extroversion.aspect3.positiveValue = active;
            extroversion.aspect4.negativeValue = reserved;
            extroversion.aspect4.positiveValue = affectionate;
            extroversion.aspect1.SetValue();
            extroversion.aspect2.SetValue();
            extroversion.aspect3.SetValue();
            extroversion.aspect4.SetValue();
            extroversion.SetTraitOverallValue();
        }
        public void InitAgreeableness()
        {
            agreeableness.aspect1.id = 5;
            agreeableness.aspect2.id = 6;
            agreeableness.aspect3.id = 7;
            agreeableness.aspect4.id = 8;
            agreeableness.aspect1.negativeValue = suspicious;
            agreeableness.aspect1.positiveValue = trusting;
            agreeableness.aspect2.negativeValue = critical;
            agreeableness.aspect2.positiveValue = lenient;
            agreeableness.aspect3.negativeValue = ruthless;
            agreeableness.aspect3.positiveValue = softHearted;
            agreeableness.aspect4.negativeValue = irritable;
            agreeableness.aspect4.positiveValue = goodNatured;
            agreeableness.aspect1.SetValue();
            agreeableness.aspect2.SetValue();
            agreeableness.aspect3.SetValue();
            agreeableness.aspect4.SetValue();
            agreeableness.SetTraitOverallValue();
        }
        public void InitConscientiousness()
        {
            conscientiousness.aspect1.id = 9;
            conscientiousness.aspect2.id = 10;
            conscientiousness.aspect3.id = 11;
            conscientiousness.aspect4.id = 12;
            conscientiousness.aspect1.negativeValue = negligent;
            conscientiousness.aspect1.positiveValue = conscientious;
            conscientiousness.aspect2.negativeValue = lazy;
            conscientiousness.aspect2.positiveValue = hardWorking;
            conscientiousness.aspect3.negativeValue = disorganized;
            conscientiousness.aspect3.positiveValue = wellOrganized;
            conscientiousness.aspect4.negativeValue = late;
            conscientiousness.aspect4.positiveValue = punctual;
            conscientiousness.aspect1.SetValue();
            conscientiousness.aspect2.SetValue();
            conscientiousness.aspect3.SetValue();
            conscientiousness.aspect4.SetValue();
            conscientiousness.SetTraitOverallValue();
        }
        public void InitNeuroticism()
        {
            neuroticism.aspect1.id = 13;
            neuroticism.aspect2.id = 14;
            neuroticism.aspect3.id = 15;
            neuroticism.aspect4.id = 16;
            neuroticism.aspect1.negativeValue = calm;
            neuroticism.aspect1.positiveValue = worried;
            neuroticism.aspect2.negativeValue = evenTempered;
            neuroticism.aspect2.positiveValue = temperamental;
            neuroticism.aspect3.negativeValue = comfortable;
            neuroticism.aspect3.positiveValue = selfConscious;
            neuroticism.aspect4.negativeValue = unemotional;
            neuroticism.aspect4.positiveValue = emotional;
            neuroticism.aspect1.SetValue();
            neuroticism.aspect2.SetValue();
            neuroticism.aspect3.SetValue();
            neuroticism.aspect4.SetValue();
            neuroticism.SetTraitOverallValue();
        }
        public void InitOpenness()
        {
            openness.aspect1.id = 17;
            openness.aspect2.id = 18;
            openness.aspect3.id = 19;
            openness.aspect4.id = 20;
            openness.aspect1.negativeValue = downToEarth;
            openness.aspect1.positiveValue = imaginative;
            openness.aspect2.negativeValue = uncreative;
            openness.aspect2.positiveValue = creative;
            openness.aspect3.negativeValue = conventional;
            openness.aspect3.positiveValue = original;
            openness.aspect4.negativeValue = uncurious;
            openness.aspect4.positiveValue = curious;
            openness.aspect1.SetValue();
            openness.aspect2.SetValue();
            openness.aspect3.SetValue();
            openness.aspect4.SetValue();
            openness.SetTraitOverallValue();
        }
        public void InitObject()
        {
            endTime = 99999;
            hunger.value = 100;
            social.value = 100;
            energy.value = 100;
            hunger.decreaseVal = 0.36f;
            social.decreaseVal = 0.24f;
            energy.decreaseVal = 0.12f;

            loner = Random.Range(0f, 0.3f);
            joiner = Random.Range(0f, 0.3f);
            quiet = Random.Range(0f, 0.3f);
            talkative = Random.Range(0f, 0.3f);
            passive = Random.Range(0f, 0.3f);
            active = Random.Range(0f, 0.3f);
            reserved = Random.Range(0f, 0.3f);
            affectionate = Random.Range(0f, 0.3f);
            suspicious = Random.Range(0f, 0.3f);
            trusting = Random.Range(0f, 0.3f);
            critical = Random.Range(0f, 0.3f);
            lenient = Random.Range(0f, 0.3f);
            ruthless = Random.Range(0f, 0.3f);
            softHearted = Random.Range(0f, 0.3f);
            irritable = Random.Range(0f, 0.3f);
            goodNatured = Random.Range(0f, 0.3f);
            negligent = Random.Range(0f, 0.3f);
            conscientious = Random.Range(0f, 0.3f);
            lazy = Random.Range(0f, 0.3f);
            hardWorking = Random.Range(0f, 0.3f);
            disorganized = Random.Range(0f, 0.3f);
            wellOrganized = Random.Range(0f, 0.3f);
            late = Random.Range(0f, 0.3f);
            punctual = Random.Range(0f, 0.3f);
            loner = Random.Range(0f, 0.3f);
            calm = Random.Range(0f, 0.3f);
            worried = Random.Range(0f, 0.3f);
            evenTempered = Random.Range(0f, 0.3f);
            temperamental = Random.Range(0f, 0.3f);
            comfortable = Random.Range(0f, 0.3f);
            selfConscious = Random.Range(0f, 0.3f);
            unemotional = Random.Range(0f, 0.3f);
            emotional = Random.Range(0f, 0.3f);
            downToEarth = Random.Range(0f, 0.3f);
            imaginative = Random.Range(0f, 0.3f);
            uncreative = Random.Range(0f, 0.3f);
            creative = Random.Range(0f, 0.3f);
            conventional = Random.Range(0f, 0.3f);
            original = Random.Range(0f, 0.3f);
            uncurious = Random.Range(0f, 0.3f);
            curious = Random.Range(0f, 0.3f);
        }
        void CheckMod(modification mod)
        {
            switch (mod.aspectName)
            {
                case "loner":
                    loner += mod.modifier;
                    break;
                case "joiner":
                    joiner += mod.modifier;
                    break;
                case "quiet":
                    quiet += mod.modifier;
                    break;
                case "talkative":
                    talkative += mod.modifier;
                    break;
                case "passive":
                    passive += mod.modifier;
                    break;
                case "active":
                    active += mod.modifier;
                    break;
                case "reserved":
                    reserved += mod.modifier;
                    break;
                case "affectionate":
                    affectionate += mod.modifier;
                    break;
                case "suspicious":
                    suspicious += mod.modifier;
                    break;
                case "trusting":
                    trusting += mod.modifier;
                    break;
                case "critical":
                    critical += mod.modifier;
                    break;
                case "lenient":
                    lenient += mod.modifier;
                    break;
                case "ruthless":
                    ruthless += mod.modifier;
                    break;
                case "softHearted":
                    softHearted += mod.modifier;
                    break;
                case "irritable":
                    irritable += mod.modifier;
                    break;
                case "goodNatured":
                    goodNatured += mod.modifier;
                    break;
                case "negligent":
                    negligent += mod.modifier;
                    break;
                case "conscientious":
                    conscientious += mod.modifier;
                    break;
                case "lazy":
                    lazy += mod.modifier;
                    break;
                case "hardWorking":
                    hardWorking += mod.modifier;
                    break;
                case "disorganized":
                    disorganized += mod.modifier;
                    break;
                case "wellOrganized":
                    wellOrganized += mod.modifier;
                    break;
                case "late":
                    late += mod.modifier;
                    break;
                case "punctual":
                    punctual += mod.modifier;
                    break;
                case "calm":
                    calm += mod.modifier;
                    break;
                case "worried":
                    worried += mod.modifier;
                    break;
                case "evenTempered":
                    evenTempered += mod.modifier;
                    break;
                case "temperamental":
                    temperamental += mod.modifier;
                    break;
                case "comfortable":
                    comfortable += mod.modifier;
                    break;
                case "selfConscious":
                    selfConscious += mod.modifier;
                    break;
                case "unemotional":
                    unemotional += mod.modifier;
                    break;
                case "emotional":
                    emotional += mod.modifier;
                    break;
                case "downToEarth":
                    downToEarth += mod.modifier;
                    break;
                case "imaginative":
                    imaginative += mod.modifier;
                    break;
                case "uncreative":
                    uncreative += mod.modifier;
                    break;
                case "creative":
                    creative += mod.modifier;
                    break;
                case "conventional":
                    conventional += mod.modifier;
                    break;
                case "original":
                    original += mod.modifier;
                    break;
                case "uncurious":
                    uncurious += mod.modifier;
                    break;
                case "curious":
                    curious += mod.modifier;
                    break;
                default:
                    print(mod.aspectName + " has been spelt incorrectly somewhere");
                    break;
            }
        }
        public void ApplyEvents()
        {
            backstory += name + " " + childhood.eventText + ". ";
            foreach (modification mod in childhood.modifications)
            {
                CheckMod(mod);
            }
            string nextLine = "After leaving home,";
            if (childhood.positive && backstoryEvents[0].tag == "negative")
            {
                nextLine = "Despite their positive upbringing";
            }
            else if (!childhood.positive && backstoryEvents[0].tag == "positive")
            {
                nextLine = "Despite their negative upbringing";
            }
            else if (!childhood.positive && backstoryEvents[0].tag == "negative")
            {
                nextLine = "Due to their negative upbringing";
            }
            else if (childhood.positive && backstoryEvents[0].tag == "positive")
            {
                nextLine = "Due to their positive upbringing";
            }
            backstory += nextLine + " ";
            foreach (LifeEvent lEvent in backstoryEvents)
            {
                foreach (modification mod in lEvent.modifications)
                {
                    CheckMod(mod);
                }
                backstory += lEvent.eventText + ". ";
                foreach (BranchLifeEvent branch in lEvent.backstoryBranches)
                {
                    foreach (modification mod in branch.modifications)
                    {
                        CheckMod(mod);
                    }
                    backstory += branch.eventText + " "; //+ ". ";
                }
            }
        }
        public void setDecrease()
        {
            if (extroversion.overallTraitValue > 0)
            {
                social.decreaseVal = social.decreaseVal + (extroversion.overallTraitValue / 10);
            }
            else if (extroversion.overallTraitValue < 0)
            {
                social.decreaseVal = social.decreaseVal - Mathf.Abs(extroversion.overallTraitValue / 10);
            }

            if (hardWorking > lazy)
            {
                energy.decreaseVal = energy.decreaseVal - (hardWorking / 10);
            }
            else if (lazy > hardWorking)
            {
                energy.decreaseVal = energy.decreaseVal + (lazy / 10);
            }
        }
        House CheckSHouseAvailable(List<House> homes)
        {
            foreach (House home in homes)
            {
                if (home.houseTrait == House.HouseTrait.social)
                {
                    return home;
                }
            }
            return null;
        }
        House CheckIHouseAvailable(List<House> homes)
        {
            foreach (House home in homes)
            {
                if (home.houseTrait == House.HouseTrait.isolated)
                {
                    return home;
                }
            }
            return null;
        }
        House CheckNHouseAvailable(List<House> homes)
        {
            foreach (House home in homes)
            {
                if (home.houseTrait == House.HouseTrait.neutral)
                {
                    return home;
                }
            }
            return null;
        }
        public void SetHouse(List<House> homes)
        {
            House homeToUse = null;
            if (extroversion.overallTraitValue > 0.25)
            {
                homeToUse = CheckSHouseAvailable(homes);
                if (homeToUse != null)
                {
                    home = homeToUse;
                }
                else
                {
                    homeToUse = CheckNHouseAvailable(homes);
                    if (homeToUse != null)
                    {
                        home = homeToUse;
                    }
                }
            }
            else if (extroversion.overallTraitValue < -0.25)
            {
                homeToUse = CheckIHouseAvailable(homes);
                if (homeToUse != null)
                {
                    home = homeToUse;
                }
                else
                {
                    homeToUse = CheckNHouseAvailable(homes);
                    if (homeToUse != null)
                    {
                        home = homeToUse;
                    }
                }
            }
            else if (extroversion.overallTraitValue > -0.25 && extroversion.overallTraitValue < 0.25)
            {
                homeToUse = CheckNHouseAvailable(homes);
                if (homeToUse != null)
                {
                    home = homeToUse;
                }
                else
                {
                    homeToUse = CheckSHouseAvailable(homes);
                    if (homeToUse != null)
                    {
                        home = homeToUse;
                    }
                }
            }
            if (homeToUse == null)
            {
                homeToUse = homes[Random.Range(0, homes.Count)];
                home = homeToUse;
            }
            homes.Remove(homeToUse);
        }
        public void SetWork(List<WorkPoint> workPoints)
        {
            foreach (WorkPoint wp in workPoints)
            {
                if (extroversion.overallTraitValue >= wp.exLow && extroversion.overallTraitValue <= wp.exHigh)
                {
                    if (agreeableness.overallTraitValue >= wp.agLow && agreeableness.overallTraitValue <= wp.agHigh)
                    {
                        if (conscientiousness.overallTraitValue >= wp.conLow && conscientiousness.overallTraitValue <= wp.conHigh)
                        {
                            if (neuroticism.overallTraitValue >= wp.neuroLow && neuroticism.overallTraitValue <= wp.neuroHigh)
                            {
                                if (openness.overallTraitValue >= wp.openLow && openness.overallTraitValue <= wp.openHigh)
                                {
                                    workPoint = wp;
                                    employed = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            workPoints.Remove(workPoint);
        }
        void MarkTime(int increase)
        {
            if (endTime == 99999)
            {
                endTime = (int)timer + increase;
            }
            if (timer >= endTime)
            {
                state = NPCState.DECIDING;
                endTime = 99999;
            }
        }
        NPCState CalculateWeights()
        {
            string logToAdd;
            state = NPCState.DECIDING;
            socialWeight = ((100 - social.value - Mathf.Epsilon) / 100) + extroversion.overallTraitValue;
            energyWeight = (100 - energy.value - Mathf.Epsilon) / 100;
            hungerWeight = ((100 - hunger.value - Mathf.Epsilon) / 100) * 1.5f;
            recreationWeight = 0.4f; //This needs to change
            agreeablenessWeight = agreeableness.overallTraitValue + Random.Range(-0.3f, 0.3f);
            conversationWeight = socialWeight + Random.Range(-0.3f, 0.3f);

            if (employed)
            {
                if (timer >= workTimeStart && timer <= workTimeEnd)
                {
                    workWeight = 0.75f;
                }
                else
                {
                    workWeight = -0.75f;
                }
                workWeight += conscientiousness.overallTraitValue;
            }
            else
            {
                workWeight = -999999;
            }

            float highest = energyWeight;
            state = NPCState.ENERGIZE;
            logToAdd = "Went to sleep";
            if (hungerWeight > highest)
            {
                highest = hungerWeight;
                state = NPCState.EATING;
                logToAdd = null;
            }
            if (workWeight > highest)
            {
                highest = workWeight;
                state = NPCState.WORKING;
                logToAdd = "Went to work as " + workPoint.title;
            }
            if (recreationWeight > highest)
            {
                highest = recreationWeight;
                state = NPCState.RECREATION;
                logToAdd = null;
            }
            if (logToAdd != null)
            {
                SetLogText(logToAdd, this);
            }
            return state;
        }
        void CalculateChildAction()
        {
            switch(state)
            {
                case NPCState.EATING:
                    float stealWeight = conscientiousness.overallTraitValue + Random.Range(-0.3f, 0.3f);
                    if(stealWeight < -0.5)
                    {
                        eatState = EatingState.STEALFOOD;
                        SetLogText("Stole food", this);
                    }
                    else
                    {
                        if(socialWeight > 0)
                        {
                            eatState = EatingState.EATATTAVERN;
                            SetLogText("Ate at tavern", this);
                        } else
                        {
                            eatState = EatingState.EATATHOME;
                            SetLogText("Ate at home", this);
                        }
                    }
                    break;
                case NPCState.RECREATION:
                    float hideWeight = extroversion.overallTraitValue + Random.Range(-0.2f,0.2f);
                    float exploreWeight = openness.overallTraitValue + Random.Range(-0.4f, 0.4f);
                    float fishWeight = neuroticism.overallTraitValue + Random.Range(-0.4f, 0.4f);
                    //Probably shouldnt go out of work hours
                    float marketWeight = ((extroversion.overallTraitValue + openness.overallTraitValue)/2) + Random.Range(-0.4f, 0.4f);

                    float highest = 0;
                    if (hideWeight < 0)
                    {
                        highest = Mathf.Abs(hideWeight);
                    }
                    recState = RecState.HIDEATHOME;
                    string logToAdd = "Hid at home";
                    if (exploreWeight > highest)
                    {
                        highest = exploreWeight;
                        recState = RecState.EXPLORE;
                        logToAdd = "Went exploring in the forest";
                    }
                    if (fishWeight > highest)
                    {
                        highest = fishWeight;
                        recState = RecState.FISH;
                        logToAdd = "Went fishing to relax";
                    }
                    if (marketWeight > highest)
                    {
                        highest = marketWeight;
                        recState = RecState.MARKET;
                        logToAdd = "Visited the market";
                    }
                    if (socialWeight > highest)
                    {
                        highest = socialWeight;
                        recState = RecState.TAVERN;
                        logToAdd = "Socialized at the tavern";
                    }
                    SetLogText(logToAdd, this);
                    break;
                default:
                    break;
            }
        }
        public void UpdateNPC(List<NPC> characters)
        {
            if (hunger.value < 0.01f)
            {
                hunger.value = 0.01f;
            }
            else if (hunger.value > 100)
            {
                hunger.value = 100;
            }
            if (energy.value < 0.01f)
            {
                energy.value = 0.01f;
            }
            else if (energy.value > 100)
            {
                energy.value = 100;
            }
            if (social.value < 0.01f)
            {
                social.value = 0.01f;
            }
            else if (social.value > 100)
            {
                social.value = 100;
            }
            if (hunger.needMode == 0)
            {
                hunger.value -= hunger.decreaseVal * Time.deltaTime;
            }
            if (social.needMode == 0)
            {
                social.value -= social.decreaseVal * Time.deltaTime;
            }
            if (energy.needMode == 0)
            {
                energy.value -= energy.decreaseVal * Time.deltaTime;
            }
            NPCObject.GetComponent<ThirdPersonCharacter>().SetMoveSpeed(updateValue);


            switch (state)
            {
                case NPCState.DECIDING:
                    NPCObject.GetComponent<IndividualCharacterController>().speechCanvas.SetActive(false);
                    hunger.needMode = 0;
                    energy.needMode = 0;
                    social.needMode = 0;
                    talkTarget = null;
                    if (aiControl.target != null)
                    {
                        if (aiControl.target.gameObject.GetComponent<GenericPointController>() != null)
                        {
                            aiControl.target.gameObject.GetComponent<GenericPointController>().inUse = false;
                        }
                    }
                    aiControl.target = null;
                    state = CalculateWeights();
                    CalculateChildAction();
                    break;
                case NPCState.RECREATION:
                    MarkTime(30);
                    switch (recState)
                    {
                        case RecState.EXPLORE:
                            currentActionText = "Exploring the forest";
                            if (aiControl.target == null)
                            {
                                aiControl.target = forestWaypoints[Random.Range(0, forestWaypoints.Length - 1)].transform;
                            }
                            else if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) <= 2)
                            {
                                aiControl.target = null;
                            }
                            break;
                        case RecState.FISH:
                            currentActionText = "Going fishing to relax";
                            if (aiControl.target == null)
                            {
                                int num = Random.Range(0, lakeWaypoints.Length - 1);
                                if (!lakeWaypoints[num].GetComponent<GenericPointController>().inUse)
                                {
                                    aiControl.target = lakeWaypoints[num].transform;
                                    lakeWaypoints[num].GetComponent<GenericPointController>().inUse = true;
                                }
                            }
                            break;
                        case RecState.HIDEATHOME:
                            currentActionText = "Going home to hide";
                            if (aiControl.target == null)
                            {
                                aiControl.target = home.houseObject.transform;
                            }
                            else
                            {
                                if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                                {
                                    currentActionText = "Hiding at home";
                                }
                            }
                            break;
                        case RecState.MARKET:
                            currentActionText = "Going to the market";
                            if (aiControl.target == null)
                            {
                                int num = Random.Range(0, marketWaypoints.Length - 1);
                                if (!marketWaypoints[num].GetComponent<GenericPointController>().inUse)
                                {
                                    aiControl.target = marketWaypoints[num].transform;
                                    marketWaypoints[num].GetComponent<GenericPointController>().inUse = true;
                                }
                            }
                            else
                            {
                                if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                                {
                                    currentActionText = "Visiting the market";
                                }
                            }
                            break;
                        case RecState.TAVERN:
                            currentActionText = "Going to the tavern to socialise";
                            if (aiControl.target == null)
                            {
                                aiControl.target = taverns[0].tavernPoints[Random.Range(0, taverns[0].tavernPoints.Count - 1)].transform;
                                aiControl.target.gameObject.GetComponent<GenericPointController>().inUse = true;
                            }
                            else
                            {
                                if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                                {
                                    currentActionText = "Socializing at the tavern";
                                    social.needMode = 1;
                                    social.value += social.decreaseVal * Time.deltaTime;
                                }
                            }
                            break;
                        default:
                            print("Something has broken with " + recState);
                            break;
                    }
                    break;
                case NPCState.ENERGIZE:
                    currentActionText = "Sleeping";
                    hunger.needMode = 0;
                    social.needMode = 0;
                    if (aiControl.target != home.bedPoint.transform)
                    {
                        aiControl.target = home.bedPoint.transform;
                    }
                    else
                    {
                        if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                        {
                            energy.needMode = 1;
                            energy.value += (energy.decreaseVal * 2) * Time.deltaTime;
                        }
                        if (energy.value >= 100)
                        {
                            energy.value = 100;
                            state = NPCState.DECIDING;
                        }
                    }
                    break;
                case NPCState.EATING:
                    currentActionText = "Travelling to eat";
                    energy.needMode = 0;
                    social.needMode = 0;

                    switch (eatState)
                    {
                        case EatingState.EATATHOME:
                            if (aiControl.target != home.chairPoint.transform)
                            {
                                aiControl.target = home.chairPoint.transform;
                            }
                            else
                            {
                                if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                                {
                                    currentActionText = "Eating at home";
                                    hunger.needMode = 1;
                                    hunger.value += (hunger.decreaseVal * 20) *Time.deltaTime;
                                }
                            }
                            break;
                        case EatingState.EATATTAVERN:
                            if(aiControl.target == null)
                            {
                                aiControl.target = taverns[0].tavernPoints[Random.Range(0, taverns[0].tavernPoints.Count - 1)].transform;
                                aiControl.target.gameObject.GetComponent<GenericPointController>().inUse = true;
                            } else
                            {
                                if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                                {
                                    currentActionText = "Eating at the tavern";
                                    hunger.needMode = 1;
                                    hunger.value += (hunger.decreaseVal * 20) * Time.deltaTime;
                                }
                            }
                            break;
                        case EatingState.STEALFOOD:
                            if (aiControl.target == null)
                            {
                                House targetHouse = null;
                                while (targetHouse == null)
                                {
                                    targetHouse = characters[Random.Range(0, characters.Count - 1)].home;
                                    if (targetHouse == home)
                                    {
                                        targetHouse = null;
                                    } else
                                    {
                                        aiControl.target = targetHouse.chairPoint.transform;
                                    }
                                }
                            }
                            if (Vector3.Distance(NPCObject.transform.position, aiControl.target.position) < 2)
                            {
                                currentActionText = "Stealing food";
                                hunger.needMode = 1;
                                hunger.value += (hunger.decreaseVal * 20) *Time.deltaTime;
                            }
                            break;
                        default:
                            print("Something has broke with state " + eatState);
                            break;
                    }
                    if (hunger.value >= 100)
                    {
                        hunger.value = 100;
                        state = NPCState.DECIDING;
                    }
                    break;
                case NPCState.WORKING:
                    currentActionText = "Travelling to work";
                    MarkTime(30);
                    aiControl.target = workPoint.point.transform;
                    if (Vector3.Distance(NPCObject.transform.position, workPoint.point.transform.position) <= 2)
                    {
                        currentActionText = workPoint.description;
                        energy.value -= (energy.decreaseVal * Time.deltaTime) * 1.5f;
                    }
                    break;
                case NPCState.TALKING:
                    MarkTime((int)conversationLength);
                    aiControl.target = talkTarget.NPCObject.transform;
                    if (talkTarget.talkTarget != this)
                    {
                        previousTalkTarget = talkTarget;
                        break;
                    }
                    string test;
                    if (Vector3.Distance(NPCObject.transform.position, talkTarget.NPCObject.transform.position) <= 1)
                    {
                        if (agreeablenessWeight >= 0)
                        {
                            test = "nice ";
                            NPCObject.GetComponent<IndividualCharacterController>().SetImage(0);
                        }
                        else
                        {
                            test = "mean ";
                            NPCObject.GetComponent<IndividualCharacterController>().SetImage(1);
                        }
                        currentActionText = "Talking " + test + talkTarget.name;
                        aiControl.target = NPCObject.transform;
                        NPCObject.transform.LookAt(talkTarget.NPCObject.transform);
                        social.value += (social.decreaseVal * 20) * Time.deltaTime;
                        social.needMode = 1;
                        previousTalkTarget = talkTarget;
                    }
                    break;
                default:
                    print("Something has broke");
                    break;
            }

            if (state != NPCState.TALKING && state != NPCState.DECIDING && conversationWeight > 0.3)
            {
                RaycastHit hit;
                if (Physics.Raycast(NPCObject.transform.position, NPCObject.transform.forward, out hit, 10))
                {
                    if (hit.collider.tag == "NPC")
                    {
                        if (hit.collider.gameObject != NPCObject)
                        {
                            foreach (NPC npc in characters)
                            {
                                if (npc.NPCObject == hit.collider.gameObject)
                                {
                                    if (npc.state != NPCState.TALKING && npc != previousTalkTarget)
                                    {
                                        if (npc.conversationWeight > 0.3)
                                        {
                                            SetLogText("Spoke to " + npc.name, this);
                                            SetLogText("Spoke to " + name, npc);
                                            conversationLength = 15 + (extroversion.aspect2.overallAspectValue * 6) + (npc.extroversion.aspect2.overallAspectValue * 6);
                                            npc.conversationLength = conversationLength;
                                            npc.state = NPCState.TALKING;
                                            npc.talkTarget = this;
                                            state = NPCState.TALKING;
                                            talkTarget = npc;
                                            break;
                                        } else
                                        {
                                            SetLogText("Tried to speak to " + npc.name + " but was ignored", this);
                                            SetLogText("Avoided talking to " + name, npc);
                                            previousTalkTarget = npc;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public GameObject cam;
    public GameObject npcPrefab;
    public int npcCount;
    public List<NPC> characters = new List<NPC>();
    public List<GameObject> aspectUIElements = new List<GameObject>();
    public List<GameObject> bigFiveUIElements = new List<GameObject>();
    public Slider hungerUI;
    public Slider socialUI;
    public Slider energyUI;
    public List<GameObject> characterPrefabs = new List<GameObject>();
    static public GameObject[] waypoints;
    static public GameObject[] forestWaypoints;
    static public GameObject[] lakeWaypoints;
    static public GameObject[] marketWaypoints;
    public List<WorkPoint> workPoints;
    public GameObject backstory;
    static public Text log;
    public Material nonOutline;
    static int updateValue = 1;
    static float timer = 480;
    public int intTime;
    Text timerText;
    Text updateText;
    Text actionText;
    Text employmentText;
    GameObject sun;
    Vector3 targetCamPosition;
    Quaternion targetCamRotation;
    int step = 3;
    public int focus = 0;
    Quaternion newSunRot = new Quaternion(0, 0, 0, 0);
    List<House> homes = new List<House>();
    static List<Tavern> taverns = new List<Tavern>();
    static int workTimeStart = 540;
    static int workTimeEnd = 1020;
    public int nightStart = 1050;
    public int nightEnd = 420;
    public bool gameStarted = false;
    void Start()
    {
        log = GameObject.Find("Log").GetComponentInChildren<Text>();
        sun = GameObject.Find("Sun");
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        updateText = GameObject.Find("UpdateText").GetComponent<Text>();
        actionText = GameObject.Find("ActionText").GetComponent<Text>();
        employmentText = GameObject.Find("EmploymentText").GetComponent<Text>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        forestWaypoints = GameObject.FindGameObjectsWithTag("ForestPoint");
        lakeWaypoints = GameObject.FindGameObjectsWithTag("LakePoint");
        marketWaypoints = GameObject.FindGameObjectsWithTag("MarketPoint");
        TextDataController textControl = GetComponent<TextDataController>();
        int firstNameCount = textControl.firstNameCount;
        int surnameCount = textControl.surnameCount;
        rnd = new System.Random(System.DateTime.Now.Millisecond);
        //npcCount = homes.Count;
        for (int i = 0; i < npcCount; i++)
        {
            NPC newNPC = new NPC
            {
                NPCObject = characterPrefabs[Random.Range(0, characterPrefabs.Count)]
            };
            newNPC.InitObject();
            newNPC.name = textControl.GetName(Random.Range(0, firstNameCount)) + " " + textControl.GetSurname(Random.Range(0, surnameCount));
            CreateEvents(newNPC);
            newNPC.ApplyEvents();
            newNPC.InitExtroversion();
            newNPC.InitAgreeableness();
            newNPC.InitConscientiousness();
            newNPC.InitNeuroticism();
            newNPC.InitOpenness();
            newNPC.setDecrease();
            newNPC.NPCObject = Instantiate(newNPC.NPCObject, transform);
            newNPC.aiControl = newNPC.NPCObject.GetComponent<AICharacterControl>();
            newNPC.camTarget = newNPC.NPCObject.GetComponent<IndividualCharacterController>().camTarget;
            newNPC.NPCObject.gameObject.name = newNPC.name;
            newNPC.state = NPC.NPCState.DECIDING;
            characters.Add(newNPC);
        }
        targetCamPosition = characters[focus].camTarget.transform.position;
        targetCamRotation = characters[focus].camTarget.transform.rotation;
        SetCanvas(aspectUIElements, bigFiveUIElements, backstory,characters[focus]);
        StartCoroutine("WaitForInit");
    }
    public string SetCanvasText(float overallTraitValue, string positive, string negative)
    {
        if (overallTraitValue >= -0.05 && overallTraitValue <= 0.05)
        {

            return "Neutral";
        }
        else if (overallTraitValue > 0.05 && overallTraitValue <= 0.25)
        {
            return "Slightly " + positive;
        }
        else if (overallTraitValue > 0.25 && overallTraitValue <= 0.6)
        {
            return positive;
        }
        else if (overallTraitValue > 0.6)
        {
            return "Very " + positive;
        }
        else if (overallTraitValue < -0.05 && overallTraitValue >= -0.25)
        {
            return "Slightly " + negative;
        }
        else if (overallTraitValue < -0.25 && overallTraitValue >= -0.6)
        {
            return negative;
        }
        else if (overallTraitValue < -0.6)
        {
            return "Very " + negative;
        }
        else return null;
    }
    public void SetCanvas(List<GameObject> aspectUIElements, List<GameObject> bigFiveUIElements, GameObject backStory, NPC character)
    {
        AspectUIController ui = aspectUIElements[0].GetComponent<AspectUIController>();
        ui.slider.value = character.extroversion.aspect1.overallAspectValue;
        ui.id = 1;
        ui.left.text = "Loner";
        ui.right.text = "Joiner";

        ui = aspectUIElements[1].GetComponent<AspectUIController>();
        ui.slider.value = character.extroversion.aspect2.overallAspectValue;
        ui.id = 2;
        ui.left.text = "Quiet";
        ui.right.text = "Talkative";

        ui = aspectUIElements[2].GetComponent<AspectUIController>();
        ui.slider.value = character.extroversion.aspect3.overallAspectValue;
        ui.id = 3;
        ui.left.text = "Passive";
        ui.right.text = "Active";

        ui = aspectUIElements[3].GetComponent<AspectUIController>();
        ui.slider.value = character.extroversion.aspect4.overallAspectValue;
        ui.id = 4;
        ui.left.text = "Reserved";
        ui.right.text = "Affectionate";

        ui = aspectUIElements[4].GetComponent<AspectUIController>();
        ui.slider.value = character.agreeableness.aspect1.overallAspectValue;
        ui.id = 5;
        ui.left.text = "Suspicious";
        ui.right.text = "Trusting";

        ui = aspectUIElements[5].GetComponent<AspectUIController>();
        ui.slider.value = character.agreeableness.aspect2.overallAspectValue;
        ui.id = 6;
        ui.left.text = "Critical";
        ui.right.text = "Lenient";

        ui = aspectUIElements[6].GetComponent<AspectUIController>();
        ui.slider.value = character.agreeableness.aspect3.overallAspectValue;
        ui.id = 7;
        ui.left.text = "Ruthless";
        ui.right.text = "Soft Hearted";

        ui = aspectUIElements[7].GetComponent<AspectUIController>();
        ui.slider.value = character.agreeableness.aspect4.overallAspectValue;
        ui.id = 8;
        ui.left.text = "Irritable";
        ui.right.text = "Good Natured";

        ui = aspectUIElements[8].GetComponent<AspectUIController>();
        ui.slider.value = character.conscientiousness.aspect1.overallAspectValue;
        ui.id = 9;
        ui.left.text = "Negligent";
        ui.right.text = "Conscientious";

        ui = aspectUIElements[9].GetComponent<AspectUIController>();
        ui.slider.value = character.conscientiousness.aspect2.overallAspectValue;
        ui.id = 10;
        ui.left.text = "Lazy";
        ui.right.text = "Hard Working";

        ui = aspectUIElements[10].GetComponent<AspectUIController>();
        ui.slider.value = character.conscientiousness.aspect3.overallAspectValue;
        ui.id = 11;
        ui.left.text = "Disorganized";
        ui.right.text = "Well Organized";

        ui = aspectUIElements[11].GetComponent<AspectUIController>();
        ui.slider.value = character.conscientiousness.aspect4.overallAspectValue;
        ui.id = 12;
        ui.left.text = "Late";
        ui.right.text = "Punctual";

        ui = aspectUIElements[12].GetComponent<AspectUIController>();
        ui.slider.value = character.neuroticism.aspect1.overallAspectValue;
        ui.id = 13;
        ui.left.text = "Calm";
        ui.right.text = "Worried";

        ui = aspectUIElements[13].GetComponent<AspectUIController>();
        ui.slider.value = character.neuroticism.aspect2.overallAspectValue;
        ui.id = 14;
        ui.left.text = "Even Tempered";
        ui.right.text = "Temperamental";

        ui = aspectUIElements[14].GetComponent<AspectUIController>();
        ui.slider.value = character.neuroticism.aspect3.overallAspectValue;
        ui.id = 15;
        ui.left.text = "Comfortable";
        ui.right.text = "Self Conscious";

        ui = aspectUIElements[15].GetComponent<AspectUIController>();
        ui.slider.value = character.neuroticism.aspect4.overallAspectValue;
        ui.id = 16;
        ui.left.text = "Unemotional";
        ui.right.text = "Emotional";

        ui = aspectUIElements[16].GetComponent<AspectUIController>();
        ui.slider.value = character.openness.aspect1.overallAspectValue;
        ui.id = 17;
        ui.left.text = "Unimaginative";
        ui.right.text = "Imaginative";

        ui = aspectUIElements[17].GetComponent<AspectUIController>();
        ui.slider.value = character.openness.aspect2.overallAspectValue;
        ui.id = 18;
        ui.left.text = "Uncreative";
        ui.right.text = "Creative";

        ui = aspectUIElements[18].GetComponent<AspectUIController>();
        ui.slider.value = character.openness.aspect3.overallAspectValue;
        ui.id = 19;
        ui.left.text = "Conventional";
        ui.right.text = "Original";

        ui = aspectUIElements[19].GetComponent<AspectUIController>();
        ui.slider.value = character.openness.aspect4.overallAspectValue;
        ui.id = 20;
        ui.left.text = "Uncurious";
        ui.right.text = "Curious";

        FivePanelHover ui2 = bigFiveUIElements[0].GetComponent<FivePanelHover>();
        ui2.text.text = SetCanvasText(character.extroversion.overallTraitValue, "Extroverted", "Introverted");

        ui2 = bigFiveUIElements[1].GetComponent<FivePanelHover>();
        ui2.text.text = SetCanvasText(character.agreeableness.overallTraitValue, "Agreeable", "Disagreeable");

        ui2 = bigFiveUIElements[2].GetComponent<FivePanelHover>();
        ui2.text.text = SetCanvasText(character.conscientiousness.overallTraitValue, "Moral", "Immoral");

        ui2 = bigFiveUIElements[3].GetComponent<FivePanelHover>();
        ui2.text.text = SetCanvasText(character.neuroticism.overallTraitValue, "Neurotic", "Relaxed");

        ui2 = bigFiveUIElements[4].GetComponent<FivePanelHover>();
        ui2.text.text = SetCanvasText(character.openness.overallTraitValue, "Open To Experience", "Closed To Experience");

        backStory.GetComponent<BackstoryDisplay>().SetText(character.backstory);
        log.GetComponentInChildren<Text>().text = character.logText;
    }
    void setHouseTrait()
    {
        foreach (House home in homes)
        {
            int num = home.houseObject.GetComponent<HouseController>().nearbyBuildings.Count;
            if (num == 0)
            {
                home.houseTrait = House.HouseTrait.isolated;
            }
            else if (num >= 3)
            {
                home.houseTrait = House.HouseTrait.social;
            }
            else
            {
                home.houseTrait = House.HouseTrait.neutral;
            }
            home.houseObject.GetComponent<CapsuleCollider>().enabled = false;
        }

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject b in buildings)
        {
            b.GetComponent<Collider>().enabled = false;
        }
    }
    public void ResetAll()
    {
        foreach(NPC character in characters)
        {
            character.InitExtroversion();
            character.InitAgreeableness();
            character.InitConscientiousness();
            character.InitNeuroticism();
            character.InitOpenness();
        }
    }
    public void ResetTraits()
    {
        characters[focus].InitExtroversion();
        characters[focus].InitAgreeableness();
        characters[focus].InitConscientiousness();
        characters[focus].InitNeuroticism();
        characters[focus].InitOpenness();
        SetCanvas(aspectUIElements, bigFiveUIElements, backstory, characters[focus]);
    }
    public void SetAllToVal(int val)
    {
        foreach(NPC character in characters)
        {
            AllExtroversion(val);
            AllAgreeableness(val);
            AllConscientiousness(val);
            AllNeuroticism(val);
            AllOpenness(val);
        }
    }
    public void AllExtroversion(int val)
    {
        foreach (NPC character in characters)
        {
            character.extroversion.aspect1.overallAspectValue = val;
            character.extroversion.aspect2.overallAspectValue = val;
            character.extroversion.aspect3.overallAspectValue = val;
            character.extroversion.aspect4.overallAspectValue = val;
            character.extroversion.SetTraitOverallValue();
        }
    }
    public void AllAgreeableness(int val)
    {
        foreach (NPC character in characters)
        {
            character.agreeableness.aspect1.overallAspectValue = val;
            character.agreeableness.aspect2.overallAspectValue = val;
            character.agreeableness.aspect3.overallAspectValue = val;
            character.agreeableness.aspect4.overallAspectValue = val;
            character.agreeableness.SetTraitOverallValue();
        }
    }
    public void AllConscientiousness(int val)
    {
        foreach (NPC character in characters)
        {
            character.conscientiousness.aspect1.overallAspectValue = val;
            character.conscientiousness.aspect2.overallAspectValue = val;
            character.conscientiousness.aspect3.overallAspectValue = val;
            character.conscientiousness.aspect4.overallAspectValue = val;
            character.conscientiousness.SetTraitOverallValue();
        }
    }
    public void AllNeuroticism(int val)
    {
        foreach (NPC character in characters)
        {
            character.neuroticism.aspect1.overallAspectValue = val;
            character.neuroticism.aspect2.overallAspectValue = val;
            character.neuroticism.aspect3.overallAspectValue = val;
            character.neuroticism.aspect4.overallAspectValue = val;
            character.neuroticism.SetTraitOverallValue();
        }
    }
    public void AllOpenness(int val)
    {
        foreach (NPC character in characters)
        {
            character.openness.aspect1.overallAspectValue = val;
            character.openness.aspect2.overallAspectValue = val;
            character.openness.aspect3.overallAspectValue = val;
            character.openness.aspect4.overallAspectValue = val;
            character.openness.SetTraitOverallValue();
        }
    }
    IEnumerator WaitForInit()
    {
        yield return new WaitForSeconds(1);
        setHouseTrait();
        foreach (NPC npc in characters)
        {
            npc.SetHouse(homes);
            npc.SetWork(workPoints);
            npc.NPCObject.transform.position = npc.home.bedPoint.transform.position;
        }
        gameStarted = true;
    }
    void CreateBranch(int level, List<BranchLifeEvent> branchList, LifeEvent root)
    {
        branchList = branchList.OrderBy(x => rnd.Next()).ToList();
        BranchLifeEvent newBranch = new BranchLifeEvent();
        foreach (BranchLifeEvent branch in branchList)
        {
            if (branch.branchLevel == level)
            {
                newBranch = branch;
                root.backstoryBranches.Add(newBranch);
                break;
            }
        }

        if (newBranch.nextTag == "null")
        {
            level++;
            CreateBranch(level, branchList, root);
        }
    }
    bool CheckID(NPC character, LifeEvent lEvent)
    {
        foreach (int id in character.idList)
        {
            if (id == lEvent.id)
            {
                return false;
            }
        }
        return true;
    }
    string CreateNewLifeEvent(string currentTag, NPC character)
    {
        LifeEvent newLEvent = new LifeEvent();
        lifeEvents = lifeEvents.OrderBy(x => rnd.Next()).ToList();
        if (currentTag == null)
        {
            foreach (LifeEvent lEvent in lifeEvents)
            {
                if (CheckID(character, lEvent))
                {
                    newLEvent = lEvent;
                    newLEvent.backstoryBranches.Clear();
                    character.backstoryEvents.Add(newLEvent);
                    character.idList.Add(lEvent.id);
                    break;
                }
            }

        }
        else if (currentTag == "nonMilitary")
        {
            foreach (LifeEvent lEvent in lifeEvents)
            {
                if (lEvent.tag != "military")
                {
                    if (CheckID(character, lEvent))
                    {
                        newLEvent = lEvent;
                        newLEvent.backstoryBranches.Clear();
                        character.backstoryEvents.Add(newLEvent);
                        character.idList.Add(lEvent.id);
                        break;
                    }
                }
            }
        }
        else if (currentTag == "nonAdventure")
        {
            foreach (LifeEvent lEvent in lifeEvents)
            {
                if (lEvent.tag != "adventure")
                {
                    if (CheckID(character, lEvent))
                    {
                        newLEvent = lEvent;
                        newLEvent.backstoryBranches.Clear();
                        character.backstoryEvents.Add(newLEvent);
                        character.idList.Add(lEvent.id);
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (LifeEvent lEvent in lifeEvents)
            {
                if (currentTag == lEvent.tag)
                {
                    if (CheckID(character, lEvent))
                    {
                        newLEvent = lEvent;
                        newLEvent.backstoryBranches.Clear();
                        character.backstoryEvents.Add(newLEvent);
                        character.idList.Add(lEvent.id);
                        break;
                    }
                }
            }
        }
        switch (newLEvent.tag)
        {
            case "military":
                CreateBranch(0, militaryBranch, newLEvent);
                currentTag = newLEvent.backstoryBranches[newLEvent.backstoryBranches.Count - 1].nextTag;
                break;
            case "work":
                CreateBranch(0, jobBranch, newLEvent);
                currentTag = newLEvent.backstoryBranches[newLEvent.backstoryBranches.Count - 1].nextTag;
                break;
            case "adventure":
                CreateBranch(0, adventureBranch, newLEvent);
                currentTag = newLEvent.backstoryBranches[newLEvent.backstoryBranches.Count - 1].nextTag;

                break;
            default:
                currentTag = newLEvent.nextTag;
                break;
        }
        if (currentTag != "end")
        {
            CreateNewLifeEvent(currentTag, character);
        }
        return currentTag;
    }
    void CreateEvents(NPC character)
    {
        ChildhoodLifeEvent newEvent;
        newEvent = childhoodLifeEvents[Random.Range(0, childhoodLifeEvents.Count)];
        character.childhood = newEvent;
        string currentTag = null;
        currentTag = CreateNewLifeEvent(currentTag, character);
    }
    public void setFocus(GameObject obj)
    {
        int i = 0;
        foreach (NPC npc in characters)
        {
            if (obj == npc.NPCObject)
            {
                focus = i;
                SetCanvas(aspectUIElements, bigFiveUIElements, backstory,characters[focus]);
                break;
            }
            else
            {
                i++;
            }
        }
    }
    private static string GetTime(float timeInSeconds)
    {
        int hours = ((int)timeInSeconds) / 60;
        int minutes = ((int)timeInSeconds) % 60;

        return hours + ":" + ((minutes < 10) ? "0" + minutes : minutes.ToString());
    }
    private void SetLight()
    {
        newSunRot = Quaternion.Euler(270 + (timer / 4), 0, 0);
        sun.transform.rotation = newSunRot;
    }
    public void CreateHouse(GameObject house, GameObject bedPoint, GameObject chairPoint)
    {
        House newHouse = new House();
        newHouse.houseObject = house;
        newHouse.bedPoint = bedPoint;
        newHouse.chairPoint = chairPoint;

        homes.Add(newHouse);
    }
    public void HouseButton()
    {
        cam.GetComponent<CameraController>().camState = CameraController.State.HOUSE;
        Vector3 newTarget = new Vector3(characters[focus].home.houseObject.transform.position.x, characters[focus].home.houseObject.transform.position.y + 3, characters[focus].home.houseObject.transform.position.z);
        cam.GetComponent<CameraController>().targetCamPosition = newTarget;
        cam.transform.LookAt(newTarget);
    }
    public void CreateTavern(List<GameObject> tavernPoints, List<GameObject> tables)
    {
        Tavern newTavern = new Tavern();
        newTavern.tavernPoints = tavernPoints;
        newTavern.tables = tables;
        taverns.Add(newTavern);
    }
    public void UpdateCanvas(Slider hungerUI, Slider socialUI, Slider energyUI, Text actionText, Text employmentText, Text logTextObj, NPC character)
    {
        hungerUI.value = character.hunger.value;
        socialUI.value = character.social.value;
        energyUI.value = character.energy.value;
        logTextObj.text = character.logText;
        actionText.text = character.currentActionText;
        if (character.employed)
        {
            employmentText.text = character.workPoint.title;
        }
        else
        {
            employmentText.text = "Unemployed";
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1440)
        {
            timer = 0;
        }
        SetLight();
        intTime = (int)timer;
        timerText.text = GetTime(intTime);
        updateText.text = "x" + Time.timeScale;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            focus++;
            if (focus > characters.Count - 1)
            {
                focus = 0;
            }
            SetCanvas(aspectUIElements, bigFiveUIElements, backstory,characters[focus]);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            focus--;
            if (focus < 0)
            {
                focus = characters.Count - 1;
            }
            SetCanvas(aspectUIElements, bigFiveUIElements, backstory,characters[focus]);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale++;
            if (Time.timeScale > 5)
            {
                Time.timeScale = 5;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale--;
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
        }
        //Possibly move to own function when it gets bigger
        if (gameStarted)
        {
            foreach (NPC character in characters)
            {
                character.UpdateNPC(characters);
            }
        }
        UpdateCanvas(hungerUI, socialUI, energyUI, actionText, employmentText,log,characters[focus]);
    }
    private void FixedUpdate()
    {
        //Should probably put this in a coroutine at some point
        foreach (NPC npc in characters)
        {
            npc.NPCObject.GetComponentInChildren<SkinnedMeshRenderer>().material = nonOutline;
        }
    }
    public void UpdateNPCValues(int id, float val)
    {
        switch (id)
        {
            case 1:
                characters[focus].extroversion.aspect1.overallAspectValue = val;
                characters[focus].extroversion.SetTraitOverallValue();
                break;
            case 2:
                characters[focus].extroversion.aspect2.overallAspectValue = val;
                characters[focus].extroversion.SetTraitOverallValue();
                break;
            case 3:
                characters[focus].extroversion.aspect3.overallAspectValue = val;
                characters[focus].extroversion.SetTraitOverallValue();
                break;
            case 4:
                characters[focus].extroversion.aspect4.overallAspectValue = val;
                characters[focus].extroversion.SetTraitOverallValue();
                break;
            case 5:
                characters[focus].agreeableness.aspect1.overallAspectValue = val;
                characters[focus].agreeableness.SetTraitOverallValue();
                break;
            case 6:
                characters[focus].agreeableness.aspect2.overallAspectValue = val;
                characters[focus].agreeableness.SetTraitOverallValue();
                break;
            case 7:
                characters[focus].agreeableness.aspect3.overallAspectValue = val;
                characters[focus].agreeableness.SetTraitOverallValue();
                break;
            case 8:
                characters[focus].agreeableness.aspect4.overallAspectValue = val;
                characters[focus].agreeableness.SetTraitOverallValue();
                break;
            case 9:
                characters[focus].conscientiousness.aspect1.overallAspectValue = val;
                characters[focus].conscientiousness.SetTraitOverallValue();
                break;
            case 10:
                characters[focus].conscientiousness.aspect2.overallAspectValue = val;
                characters[focus].conscientiousness.SetTraitOverallValue();
                break;
            case 11:
                characters[focus].conscientiousness.aspect3.overallAspectValue = val;
                characters[focus].conscientiousness.SetTraitOverallValue();
                break;
            case 12:
                characters[focus].conscientiousness.aspect4.overallAspectValue = val;
                characters[focus].conscientiousness.SetTraitOverallValue();
                break;
            case 13:
                characters[focus].neuroticism.aspect1.overallAspectValue = val;
                characters[focus].neuroticism.SetTraitOverallValue();
                break;
            case 14:
                characters[focus].neuroticism.aspect2.overallAspectValue = val;
                characters[focus].neuroticism.SetTraitOverallValue();
                break;
            case 15:
                characters[focus].neuroticism.aspect3.overallAspectValue = val;
                characters[focus].neuroticism.SetTraitOverallValue();
                break;
            case 16:
                characters[focus].neuroticism.aspect4.overallAspectValue = val;
                characters[focus].neuroticism.SetTraitOverallValue();
                break;
            case 17:
                characters[focus].openness.aspect1.overallAspectValue = val;
                characters[focus].openness.SetTraitOverallValue();
                break;
            case 18:
                characters[focus].openness.aspect2.overallAspectValue = val;
                characters[focus].openness.SetTraitOverallValue();
                break;
            case 19:
                characters[focus].openness.aspect3.overallAspectValue = val;
                characters[focus].openness.SetTraitOverallValue();
                break;
            case 20:
                characters[focus].openness.aspect4.overallAspectValue = val;
                characters[focus].openness.SetTraitOverallValue();
                break;
            default:
                print("something brokw with id " + id);
                break;                
        }
        SetCanvas(aspectUIElements, bigFiveUIElements, backstory, characters[focus]);
    }
    static private void SetLogText(string text, NPC character)
    {
        character.logText += GetTime(timer) + ": " + text + "\n";
        log.GetComponentInChildren<Text>().text = character.logText;
    }
}
