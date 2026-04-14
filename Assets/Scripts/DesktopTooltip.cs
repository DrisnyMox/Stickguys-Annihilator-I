using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopTooltip : MonoBehaviour
{
    [SerializeField] GameObject tooltip;

    bool isMobile;

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    // Update is called once per frame
    void Update()
    {
        tooltip.SetActive(!isMobile);
    }
}
