using Nenn.InspectorEnhancements.Runtime.Attributes;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class LevelEdittor : MonoBehaviour
{
   public LevelDesignObject[] levelDesignObjects;
   [SerializeField] GridStateSystem gridStateSystem;
   [Header("GridWord")]
   [SerializeField] int2 gridSize;
   [SerializeField] float2 scale;
   [SerializeField] float3 centerPos;
   [SerializeField] Difficulty difficulty;
   [SerializeField] int[] colorValues;
   [SerializeField] AvailableBlockEditor[] availableBlocks;
   [SerializeField] int ratioDoubleAvailableBlock;
   [SerializeField] int amountBlock;

   [Range(1, 10)]
   [SerializeField] int levelSelection = 1;
   LevelDesignObject CurrentLevelDesignObject => levelDesignObjects[levelSelection - 1];

   [MethodButton]
   public void Clear()
   {
      gridStateSystem.CreateGrid(gridSize, scale, centerPos);
      colorValues = null;
      availableBlocks = null;
      ratioDoubleAvailableBlock = 0;
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
         levelDesignObjects[i].difficulty = _levelDesignData.data[i].difficulty;
         levelDesignObjects[i].colorValues = _levelDesignData.data[i].colorValues;
         levelDesignObjects[i].availableBlocks = _levelDesignData.data[i].availableBlocks;
         levelDesignObjects[i].ratioDoubleAvailableBlock = _levelDesignData.data[i].ratioDoubleAvailableBlock;
         levelDesignObjects[i].amountBlock = _levelDesignData.data[i].amountBlock;
      }
   }
   [MethodButton]
   public void Load()
   {
      gridSize = CurrentLevelDesignObject.gridSize;
      scale = CurrentLevelDesignObject.scale;
      centerPos = CurrentLevelDesignObject.centerPos;
      difficulty = (Difficulty)CurrentLevelDesignObject.difficulty;
      colorValues = CurrentLevelDesignObject.colorValues;
      ratioDoubleAvailableBlock = CurrentLevelDesignObject.ratioDoubleAvailableBlock;
      amountBlock = CurrentLevelDesignObject.amountBlock;

      availableBlocks = new AvailableBlockEditor[CurrentLevelDesignObject.availableBlocks.Length];
      for (int i = 0; i < availableBlocks.Length; i++)
      {
         availableBlocks[i].GRIDSTATE = (GRIDSTATE)CurrentLevelDesignObject.availableBlocks[i].GRIDSTATE;
         availableBlocks[i].ratio = CurrentLevelDesignObject.availableBlocks[i].ratio;
      }

      gridStateSystem.CreateGrid(gridSize, scale, centerPos);
      gridStateSystem.InitData(CurrentLevelDesignObject);
   }
   [MethodButton]
   public void Save()
   {
      CurrentLevelDesignObject.gridSize = gridSize;
      CurrentLevelDesignObject.scale = scale;
      CurrentLevelDesignObject.centerPos = centerPos;
      CurrentLevelDesignObject.difficulty = (int)difficulty;
      CurrentLevelDesignObject.colorValues = colorValues;
      CurrentLevelDesignObject.ratioDoubleAvailableBlock = ratioDoubleAvailableBlock;
      CurrentLevelDesignObject.amountBlock = amountBlock;

      var availableBlocks = new AvailableBlock[this.availableBlocks.Length];
      for (int i = 0; i < availableBlocks.Length; i++)
      {
         availableBlocks[i].GRIDSTATE = (int)this.availableBlocks[i].GRIDSTATE;
         availableBlocks[i].ratio = this.availableBlocks[i].ratio;
      }
      CurrentLevelDesignObject.availableBlocks = availableBlocks;

      var gridStateControls = gridStateSystem.gridStateControls;
      var grids = new Grid[gridStateControls.Length];
      for (int i = 0; i < grids.Length; i++)
      {
         grids[i].GRIDSTATE = (int)gridStateControls[i].GRIDSTATE;
         grids[i].ColorIndexs = gridStateControls[i].ColorIndexs;
      }
      CurrentLevelDesignObject.grids = grids;

#if UNITY_EDITOR
      EditorUtility.SetDirty(CurrentLevelDesignObject);
      AssetDatabase.SaveAssets();
#endif

      var _data = new LevelDesignData[levelDesignObjects.Length];
      for (int i = 0; i < _data.Length; i++)
      {
         _data[i].gridSize = levelDesignObjects[i].gridSize;
         _data[i].scale = levelDesignObjects[i].scale;
         _data[i].centerPos = levelDesignObjects[i].centerPos;
         _data[i].difficulty = levelDesignObjects[i].difficulty;
         _data[i].colorValues = levelDesignObjects[i].colorValues;
         _data[i].grids = levelDesignObjects[i].grids;
         _data[i].availableBlocks = levelDesignObjects[i].availableBlocks;
         _data[i].ratioDoubleAvailableBlock = levelDesignObjects[i].ratioDoubleAvailableBlock;
         _data[i].amountBlock = levelDesignObjects[i].amountBlock;
      }
      var _levelDesignDatas = new LevelDesignDatas()
      {
         data = _data
      };
      Utility.SaveToFile<LevelDesignDatas>(_levelDesignDatas, KeyStr.NAME_FILE_LEVEL_DESIGN_TXT);
      Debug.Log("Save success");
   }
}
