using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ImpactFactors
{
    public float TermoFactor;         //Угроза: Температурный фактор;                                                   Протеводействие: Система температурного конторля
    public float RealityFactor;       //Угроза: Фактор воздействия на текущую реальность и взаимодействия с другими;    Протеводействие: Фиксирование реальности
    public float MentalFactor;        //Угроза: Фактор воздействия на разум;                                            Протеводействие: Ментальное изолирование
    public float PhisicalFactor;      //Угроза: Фактор физических повреждений;                                          Протеводействие: Укрепление
    public float SpaceFactor;         //Угроза: Фактор воздействия на пространство;                                     Протеводействие: Стабилизация пространства
    public float RadiationFactor;     //Угроза: Фактор излучения;                                                       Протеводействие: Экранирование
    public float WillFactor;          //Угроза: Фактор воли;                                                            Протеводействие: Наблюдение и контроль
    public float BiohazardFactor;     //Угроза: Химическое / биологическое заражение;                                   Противодействие: Гермитизация

    public readonly static float MaxValue = 10;
    public readonly static float FieldsCount = 8;

    public ImpactFactors(
        float TermoFactor,
        float RealityFactor,
        float MentalFactor,
        float PhisicalFactor,
        float SpaceFactor,
        float RadiationFactor,
        float WillFactor,
        float BiohazardFactor)
    {
        this.TermoFactor = TermoFactor;
        this.RealityFactor = RealityFactor;
        this.MentalFactor = MentalFactor;
        this.PhisicalFactor = PhisicalFactor;
        this.SpaceFactor = SpaceFactor;
        this.RadiationFactor = RadiationFactor;
        this.WillFactor = WillFactor;
        this.BiohazardFactor = BiohazardFactor;
    }

    public ImpactFactors(float CommonValue)
    {
        TermoFactor = CommonValue;
        RealityFactor = CommonValue;
        MentalFactor = CommonValue;
        PhisicalFactor = CommonValue;
        SpaceFactor = CommonValue;
        RadiationFactor = CommonValue;
        WillFactor = CommonValue;
        BiohazardFactor = CommonValue;
    }

    public float this[int i]
    {
        get
        {
            switch (i)
            {
                case (0): return TermoFactor;
                case (1): return RealityFactor;
                case (2): return MentalFactor;
                case (3): return PhisicalFactor;
                case (4): return SpaceFactor;
                case (5): return RadiationFactor;
                case (6): return WillFactor;
                case (7): return BiohazardFactor;
                default: return 0;
            }
        }
        set
        {
            switch (i)
            {
                case (0): TermoFactor = value; return;
                case (1): RealityFactor = value; return;
                case (2): MentalFactor = value; return;
                case (3): PhisicalFactor = value; return;
                case (4): SpaceFactor = value; return;
                case (5): RadiationFactor = value; return;
                case (6): WillFactor = value; return;
                case (7): BiohazardFactor = value; return;
            }
        }
    }

    public static explicit operator float(ImpactFactors A)
    {
        float result = 0;
        for (int i = 0; i < FieldsCount; i++)
            result += A[i];
        return result;
    }

    public static ImpactFactors operator+ (ImpactFactors A, ImpactFactors B)
    {
        ImpactFactors result = new ImpactFactors(0);
        for (int i = 0; i < FieldsCount; i++)
            result[i] = A[i] + B[i];
        return result;
    }

    public static ImpactFactors operator -(ImpactFactors A, ImpactFactors B)
    {
        ImpactFactors result = new ImpactFactors(0);
        for (int i = 0; i < FieldsCount; i++)
        {
            result[i] = A[i] - B[i];
            if (result[i] < 0) result[i] = 0;
        }
        return result;
    }

    public static ImpactFactors operator* (ImpactFactors A, ImpactFactors B)
    {
        ImpactFactors result = new ImpactFactors(0);
        for (int i = 0; i < FieldsCount; i++)
            result[i] = A[i] * B[i];
        return result;
    }

    public static ImpactFactors operator* (ImpactFactors A, float c)
    {
        ImpactFactors result = new ImpactFactors(0);
        for (int i = 0; i < FieldsCount; i++)
            result[i] = A[i] * c;
        return result;
    }
}