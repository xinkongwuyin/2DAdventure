using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();
    void RegisterSaveData()
    {
        DataManager.instance.RegisterSaveData(this);
    }

    void UnRegisterSaveData()=>DataManager.instance.UnRegisterSaveData(this); //¼òĞ´

    void GetLoadData(Data data);

    void LoadData(Data data);
}
