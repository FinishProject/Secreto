﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System;


public class CSVParser : CSVReader
{
    public string fileName { set; get;}

    public CSVParser() { }
    public CSVParser(string fileName) { this.fileName = fileName; }

    public override void Load()
    {
        OpenFile(fileName);
    }

    //===========================   아이템   ===========================
    // 아이디로 검색, 반환
    public void ParseByID(ItemStruct data, int id)
    {
        for (int row = 2; row < rowCnt; row++)
        {
            if(!Convert.ToInt32(stringList[colCnt * row ]).Equals(id))
                continue;

            data.SetData(
                Convert.ToInt32 (stringList[colCnt * row]),
                Convert.ToString(stringList[colCnt * row + 1]),
                Convert.ToInt32 (stringList[colCnt * row + 2]),
                Convert.ToInt32 (stringList[colCnt * row + 3]));

            break;
        }
    }

    // 이름으로 검색, 반환
    public void ParseByName(ItemStruct data, string name)
    {
        for (int row = 2; row < rowCnt; row++)
        {
            if (!Convert.ToString(stringList[colCnt * row +1 ]).Equals(name))
                continue;

            data.SetData(
                Convert.ToInt32(stringList[colCnt * row]),
                Convert.ToString(stringList[colCnt * row + 1]),
                Convert.ToInt32(stringList[colCnt * row + 2]),
                Convert.ToInt32(stringList[colCnt * row + 3]));

            break;
        }
    }

    //===========================   경험치 테이블   ===========================
    // 레벨로 검색, 반환
    public void ParseByLevel(ExpTable data, int level)
    {
        for (int row = 2; row < rowCnt; row++)
        {
            if (!Convert.ToInt32(stringList[colCnt * row]).Equals(level))
                continue;

            data.SetData(
                Convert.ToInt32(stringList[colCnt * row]),
                Convert.ToInt32(stringList[colCnt * row + 1]));

            break;
        }
    }

    public void Save()
    {
        int width = Convert.ToInt32(stringList[0]);
        string[][] output = new string[stringList.Length / 2][];

        for (int i = 0; i < output.Length; i++)
        {
            string[] temp = new string[width];
            for (int j = 0; j < width; j++)
            {
                temp[j] = stringList[i * width + j];
                output[i] = temp;
            }
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        sb.Remove(sb.Length - 2, 2);

        string filePath = Application.streamingAssetsPath + "/" + fileName + ".csv";
        File.WriteAllText(filePath, sb.ToString());

    }
}
