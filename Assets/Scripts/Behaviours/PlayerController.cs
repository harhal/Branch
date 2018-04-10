using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController MainController;

    public GraphicRaycaster MainUIRaycaster;
    public Image OverlayBG;
    public Transform Canvas;

    public enum ControlMode { Default, Build, AnomalObjectControl, BuildingControl, OverlayWindow }
    public ControlMode CurrentMode
    {
        get;
        private set;
    }
    ControlMode LastMode;
    public Vector3 FlyOffset = new Vector3(0, 1, 0);
    public List<BlockingPanel> blockingInputPanels;

    public BuildingGameObject PlacingBuilding { get; private set; }
    public AnomalObject PlacingObject { get; private set; }
    public Building ControllingBuilding { get; private set; }
    public Stack<OverlayWindow> OverlayStack { get; private set; }
    float LastTimeScale; 

    bool IsInputBlocked(Vector2 input)
    {
        bool result = false;
        foreach (BlockingPanel panel in blockingInputPanels)
        {
            if (panel.IsOverlapPoint(input))
                result = true;
        }
        return result;
    }

    public bool IsBuildingSelected(Building building)
    {
        return ControllingBuilding == building;
    }

    public bool IsAnomalObjectSelected(AnomalObject anomalObject)
    {
        return PlacingObject == anomalObject;
    }

    public void ShowReport(Report report)
    {
        ReportUI.UI.SetReport(report);
        AddOverlayWindow(ReportUI.UI);
    }

    public void ShowHuman(Human human)
    {
        PeopleUI.UI.ShowPeople(null);
        PeopleUI.UI.ShowHumanInfo(human);
        AddOverlayWindow(PeopleUI.UI);
    }

    public void ShowPeople(int ProfessionFilter)
    {
        if (ProfessionFilter >= 0)
            PeopleUI.UI.ShowPeople((Human.ProfessionType)ProfessionFilter);
        AddOverlayWindow(PeopleUI.UI);
    }

    public void ShowPeopleControl(string Label, List<Human> FreePeople, List<Human> HiredPeople, int MaxHiredCount, PeopleControlUI.ConfirmDelegate OnConfirm)
    {
        PeopleControlUI.UI.ShowPeople(Label, FreePeople, HiredPeople, MaxHiredCount, OnConfirm);
        AddOverlayWindow(PeopleControlUI.UI);
    }

    public void ShowPeopleControl(string Label, List<Human> FreePeople, List<int> HiredPeople, int MaxHiredCount, PeopleControlUI.ConfirmDelegate OnConfirm)
    {
        PeopleControlUI.UI.ShowPeople(Label, FreePeople, HiredPeople, MaxHiredCount, OnConfirm);
        AddOverlayWindow(PeopleControlUI.UI);
    }

    public void ShowUpgrade(ObjectStorage storage)
    {
        ObjectStorageUpgradeUI.UI.SetStorage(storage);
        AddOverlayWindow(ObjectStorageUpgradeUI.UI);
    }

    public void ShowTech()
    {
        ResearchesUI.UI.Show();
        AddOverlayWindow(ResearchesUI.UI);
    }

    public void ShowMessage(string message)
    {
        TextMessageUI.UI.SetMessage(message);
    }

    void ResetMode()
    {
        //print("ControlReset");
        switch (CurrentMode)
        {
            case (ControlMode.Default):
                {
                    //BottomPanelSwitch.UI.SwitchToBuild();
                    break;
                }
            case (ControlMode.Build):
                {
                    GameObject.Destroy(PlacingBuilding.gameObject);
                    Builder.builder.SetEnabledResetButton(false);
                    break;
                }
            case (ControlMode.AnomalObjectControl):
                {
                    AnomalObjectUI.FirstUI.Hide();
                    break;
                }
            case (ControlMode.BuildingControl):
                {
                    if (ControllingBuilding is SoilBlock)
                    {
                        SoilUI.UI.Hide();
                    }
                    else if (ControllingBuilding is BuildingInProcess)
                    {
                        BuildingProcessUI.UI.Hide();
                    }
                    else if (ControllingBuilding is ObjectStorage)
                    {
                        ObjectStorageUI.UI.Hide();
                        AnomalObjectUI.FirstUI.Hide();
                    }
                    break;
                }
            case (ControlMode.OverlayWindow):
                {
                    break;
                }
        }
    }

    public void SetDefaultMode()
    {
        CameraControl.MainCamera.Free();
        ResetMode();
        GridComponent.grid.HighLightOff();
        CurrentMode = ControlMode.Default;
    }

    public void SetBuildMode(BuildingGameObject prototype)
    {
        CameraControl.MainCamera.Free();
        ResetMode();
        PlacingBuilding = prototype;
        GridComponent.grid.HighLightForBuilding();
        Builder.builder.SetEnabledResetButton(true);
        CurrentMode = ControlMode.Build;
    }

    public void SetBuildingControlMode(Building building)
    {
        ResetMode();
        ControllingBuilding = building;
        if (building is SoilBlock)
            SoilUI.UI.SetData(building as SoilBlock);
        if (building is BuildingInProcess)
            BuildingProcessUI.UI.SetProcess(building as BuildingInProcess);
        if (building is ObjectStorage)
            ObjectStorageUI.UI.SetObjectStorage(building as ObjectStorage);
        CameraControl.MainCamera.FocusAt(GridComponent.grid.Buildings[building].transform.position);
        GridComponent.grid.HighLightFocus(building);
        CurrentMode = ControlMode.BuildingControl;
    }

    public void SetAnomalObjectMode(AnomalObject anomalObject)
    {
        ResetMode();
        CameraControl.MainCamera.Free();
        PlacingObject = anomalObject;
        AnomalObjectUI.FirstUI.SetAnomalObject(anomalObject);
        CurrentMode = ControlMode.AnomalObjectControl;
        GridComponent.grid.HighLightFreeStorages();
    }

    public void AddOverlayWindow(OverlayWindow overlayWindow)
    {
        if (OverlayStack.Count <= 0)
        {
            LastTimeScale = Time.timeScale;
            TimeController.timeController.SetTimeScale(0);
            CameraControl.MainCamera.enabled = false;
            MainUIRaycaster.enabled = false;
            OverlayBG.enabled = true;
            LastMode = CurrentMode;
            CurrentMode = ControlMode.OverlayWindow;
        }
        OverlayStack.Push(overlayWindow);
            OverlayBG.transform.SetSiblingIndex(Canvas.childCount - 1);
            overlayWindow.transform.SetSiblingIndex(Canvas.childCount - 1);
        overlayWindow.Show();
    }

    public void CloseTopOverlayWindow()
    {
        if (OverlayStack.Count > 0)
        {
            var window = OverlayStack.Pop();
            window.Hide();
            if (OverlayStack.Count <= 0)
            {
                TimeController.timeController.SetTimeScale(LastTimeScale);
                CameraControl.MainCamera.enabled = true;
                MainUIRaycaster.enabled = true;
                OverlayBG.enabled = false;
                CurrentMode = LastMode;
            }
        }
    }

    private void Awake()
    {
        if (MainController == null)
            MainController = this;
        blockingInputPanels = new List<BlockingPanel>();
        OverlayStack = new Stack<OverlayWindow>();
        LastTimeScale = 1;
        OverlayBG.enabled = false;
    }

    Vector2Int? GetCellLocation()
    {
        RaycastHit hit;
        Ray ray = CameraControl.MainCamera.camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 100);
        if (!Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << 8)) return null;
        if (GridComponent.grid.gameObject != hit.collider.gameObject) return null;
        return GridComponent.grid.LocationToGrid(hit.point);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentMode != ControlMode.OverlayWindow)
                SetDefaultMode();
            else
                CloseTopOverlayWindow();
        }
        switch (CurrentMode)
        {
            case ControlMode.Default:
                {
                    if (Input.GetMouseButtonDown(0) && !IsInputBlocked(Input.mousePosition))
                    {
                        Vector2Int? cellLocation = GetCellLocation();
                        if (cellLocation == null) return;
                        var Selection = GridComponent.grid.GetBuildingAt(cellLocation.Value);
                        if (Selection != null)
                        {
                            SetBuildingControlMode(Selection);
                        }
                    }
                    break;
                }
            case ControlMode.Build:
                {
                    Vector2Int? cellLocation = GetCellLocation();
                    if (cellLocation == null) return;
                    PlacingBuilding.transform.position = GridComponent.grid.GridToLocation(cellLocation.Value) + FlyOffset;
                    bool IsPossible = GridComponent.grid.CheckPlace(PlacingBuilding.Building, cellLocation.Value);
                    if (!IsPossible && PlacingBuilding.BuildingRenderer != null)
                        PlacingBuilding.BuildingRenderer.material.color = Color.red;
                    else
                        PlacingBuilding.BuildingRenderer.material.color = Color.white;
                    if (Input.GetMouseButtonDown(0) && IsPossible && !IsInputBlocked(Input.mousePosition))
                        Build(cellLocation.Value);
                    break;
                }
            case ControlMode.AnomalObjectControl:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2Int? cellLocation = GetCellLocation();
                        if (cellLocation == null) return;
                        var storage = GridComponent.grid.GetBuildingAt(cellLocation.Value) as ObjectStorage;
                        if (storage != null)
                        {
                            if (storage.anomalObject == null)
                            {
                                var anomalObject = PlacingObject;
                                if (anomalObject != null)
                                {
                                    if (anomalObject.Storage != null)
                                    {
                                        foreach ( var item in anomalObject.Storage.Scientists)
                                        {
                                            SessionData.Data.ResourceStorage.People[item].Fire();
                                        }
                                        anomalObject.Storage.Scientists.Clear();
                                        foreach (var item in anomalObject.Storage.Guard)
                                        {
                                            SessionData.Data.ResourceStorage.People[item].Fire();
                                        }
                                        anomalObject.Storage.Guard.Clear();
                                        anomalObject.Storage.anomalObject = null;
                                    }
                                    storage.anomalObject = anomalObject;
                                    anomalObject.Storage = storage;
                                    EntrepotUI.UI.RefreshData();
                                }
                            }
                            SetBuildingControlMode(storage);
                        }
                    }
                    break;
                }
            case ControlMode.BuildingControl:
                {
                    break;
                }
            case ControlMode.OverlayWindow:
                {
                    break;
                }
        }
    }

    void Build(Vector2Int CellLocation)
    {
        if (PlacingBuilding == null) return;
        if (PlacingBuilding.Building.Cost <= SessionData.Data.ResourceStorage.Money)
        {
            SessionData.Data.ResourceStorage.SpendMoney(PlacingBuilding.Building.Cost);
            (PlacingBuilding.Building as BuildingInProcess).IsActive = true;
            if ((PlacingBuilding.Building as BuildingInProcess).NextBuilding is ObjectStorage)
            {
                ((PlacingBuilding.Building as BuildingInProcess).NextBuilding as ObjectStorage).ID = SessionData.Data.BranchBase.GetNewStorageID();
            }
            GridComponent.grid.PlaceBuilding(PlacingBuilding.Building, CellLocation);
            SetBuildingControlMode(PlacingBuilding.Building);
            Kanban.Board.BuildingProcessStarted("Building in process", PlacingBuilding.Building);
        }
        else
        {
            ShowMessage("You don't have enought money");
        }
    }
}