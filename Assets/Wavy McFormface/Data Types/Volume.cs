using UnityEngine;

namespace bsgbryan {

  /*
    A simple wrapper for volume level that allows for
    a custom PropertyDrawer and supports Lerping
   */
  [System.Serializable]
  public class Volume {

    /*
      The volume level you want to specify
     */
    public float Level;

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public float CurrentLevel;

    /*
      !!! DO NOT MANIPULATE THIS FIELD DIRECTLY !!!
      This field is used internally by MavyMcFormface
      for Lerping
     */
    [System.NonSerialized]
    public float PreviousLevel;

  }
}
