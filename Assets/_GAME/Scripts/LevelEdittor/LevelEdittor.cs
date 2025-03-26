using Nenn.InspectorEnhancements.Runtime.Attributes;
using Unity.Mathematics;
using UnityEngine;

public class LevelEdittor : MonoBehaviour
{
   public LevelDesignObject[] levelDesignObjects;
   [SerializeField] GridStateSystem gridStateSystem;
   [Header("GridWord")]
   [SerializeField] int2 gridSize;
   [SerializeField] float2 scale;
   [SerializeField] float3 centerPos;

   [Range(1, 10)]
   [SerializeField] int levelSelection;
   LevelDesignObject CurrentLevelDesignObject => levelDesignObjects[levelSelection - 1];

   [MethodButton]
   public void CreateGrid()
   {
      gridStateSystem.CreateGrid(gridSize, scale, centerPos);
   }
   [MethodButton]
   public void LoadLevelFromDisk()
   {
      var _levelDesignData = Utility.LoadDataFromFile<LevelDesignDatas>(KeyStr.NAME_FILE_LEVEL_DESIGN_TXT);
      for (int i = 0; i < _levelDesignData.data.Length; i++)
      {
         levelDesignObjects[i].grids = _levelDesignData.data[i].grids;
         levelDesignObjects[i].gridSize = _levelDesignData.data[i].gridSize;
         levelDesignObjects[i].scale = _levelDesignData.data[i].scale;
         levelDesignObjects[i].centerPos = _levelDesignData.data[i].centerPos;
      }
   }
   [MethodButton]
   public void Load()
   {
      gridSize = CurrentLevelDesignObject.gridSize;
      scale = CurrentLevelDesignObject.scale;
      centerPos = CurrentLevelDesignObject.centerPos;
      CreateGrid();
      gridStateSystem.InitData(CurrentLevelDesignObject);
   }
   [MethodButton]
   public void Save()
   {
      CurrentLevelDesignObject.gridSize = gridSize;
      CurrentLevelDesignObject.scale = scale;
      CurrentLevelDesignObject.centerPos = centerPos;

      var gridStateControls = gridStateSystem.gridStateControls;
      var grids = new Grid[gridStateControls.Length];
      for (int i = 0; i < grids.Length; i++)
      {
         grids[i].GRIDSTATE = gridStateControls[i].GRIDSTATE;
         grids[i].ColorIndexs = gridStateControls[i].ColorIndexs;
      }
      CurrentLevelDesignObject.grids = grids;

      var _data = new LevelDesignData[levelDesignObjects.Length];
      for (int i = 0; i < _data.Length; i++)
      {
         _data[i].gridSize = levelDesignObjects[i].gridSize;
         _data[i].scale = levelDesignObjects[i].scale;
         _data[i].centerPos = levelDesignObjects[i].centerPos;
         _data[i].grids = levelDesignObjects[i].grids;
      }
      var _levelDesignDatas = new LevelDesignDatas()
      {
         data = _data
      };
      Utility.SaveToFile<LevelDesignDatas>(_levelDesignDatas,KeyStr.NAME_FILE_LEVEL_DESIGN_TXT);
   }
}
