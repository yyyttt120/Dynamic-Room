using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* attached onto the data operator of ML classifier */
public class Find4Walls : MonoBehaviour
{
    public ML_Block[] blocks;
    public ML_Block currentBlock { set; get; }
    public bool blockChanged; //turn true, when the current block vary
    private ML_Block lastBlock;
    // Start is called before the first frame update
    void Start()
    {
        currentBlock = blocks[0];
        lastBlock = currentBlock;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentBlock();
        //print($"current block = {currentBlock.name}");
        if (lastBlock == currentBlock)
            blockChanged = false;
        else
            blockChanged = true;
        lastBlock = currentBlock;
    }

    public void UpdateCurrentBlock()
    {
        foreach (ML_Block block in blocks)
        {
            if(block.userEntered)
            {
                currentBlock = block;
                break;
            }
        }
    }
}
