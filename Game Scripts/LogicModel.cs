using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogicModel
{
    public static int Check21(TableSeat seat) // calculates the sum of the cards at the table
    {
        int count = 0;
        int AceCount = 0;
        foreach (CardObject c in seat.seatCards)
        {
            count += c.value;

            if (c.name.Contains("A"))
            {
                AceCount += 1;
            }
        }
        if (count > 21 && AceCount > 0)
        {
            count -= AceCount * 10;
        }
        return count;
    }


    public static string DealerDecision(TableSeat seat)
    {
        int count = Check21(seat);
        if (count <= 16)
        {
            return "HIT";
        }
        else if (count > 16 && count < 21)
        {
            return "STAY";
        }
        else if (count == 21)
        {
            return "BLACKJACK";
        }
        else
        {
            return "BUST";
        }
    }

}
