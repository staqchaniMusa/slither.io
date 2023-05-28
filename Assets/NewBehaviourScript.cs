// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
      float Basic=0;
    float HRA=0;
    float CCA=0;
    float Conveyance_Allowance = 1600;
    float Gross=0;
    float Employer_PF=0;
    float Employer_ESIC=0;
    float Employee_PF=0;
    float Employee_ESIC=0;
    float Takehome=0;
    


    // Make Constants editable. ( 0.75f , 3.25f , 21000 , 23067f etc) are right current value , If we have to change, we need to change them once to calculate all salaries.

    const float EMPLOYER_PF_PERCENTAGE = 12f;
    const float EMPLOYER_ESIC_PERCENTAGE = 3.25f;
    

    const float EMPLOYEE_PF_PERCENTAGE = 12f;
    const float EMPLOYEE_ESIC_PERCENTAGE = 0.75f;

    
    const float BASIC_PERCENTAGE = 50;
    const float HRA_PERCENTAGE = 50;

    const float ESIC_CUTOFF = 21000f;
    const float MAXIMUM_CTC_FOR_ESIC_CUTOFF = 23067f;

   
    void CalcalateSalryStuff(float CTC)
    {
        
        if (CTC >= 10000 && CTC < 15000) {
                Basic = (CTC * 75)/100;
                HRA= (float)(CTC * 17.51)/100;
                Conveyance_Allowance = 0;
                CCA= 0 ;
                Gross = Basic+HRA+Conveyance_Allowance+CCA;
                Employer_PF = (Basic * 12)/100;
                Employer_ESIC= (float)(Gross*3.25)/100;
                Employee_PF= (Basic * 12)/100;
                Employee_ESIC= (float) (Gross*0.75)/100;
                Takehome= Gross - Employee_PF - Employee_ESIC;
        
        }

        
        
        
        if (CTC>=15000) {
        
        
        Basic = (CTC * BASIC_PERCENTAGE) / 100;
       

        HRA = (Basic * HRA_PERCENTAGE) / 100;
     

        Gross = CTC >= MAXIMUM_CTC_FOR_ESIC_CUTOFF ? CTC - Math.Min((Basic * EMPLOYER_PF_PERCENTAGE) / 100, 1800) : (CTC * (100f - (EMPLOYER_PF_PERCENTAGE)/2)) / (100f + EMPLOYER_ESIC_PERCENTAGE);


        CCA = Gross - Basic - HRA - Conveyance_Allowance;
     

        Employer_PF = Math.Min(1800, (Basic* EMPLOYER_PF_PERCENTAGE)/100);
      

        Employer_ESIC = Gross > ESIC_CUTOFF ? 0:(Gross * EMPLOYER_ESIC_PERCENTAGE) / 100;
    

        Employee_PF = Math.Min(1800, (Basic* EMPLOYEE_PF_PERCENTAGE)/100);;
     

        Employee_ESIC = Gross > ESIC_CUTOFF ? 0 : (Gross * EMPLOYEE_ESIC_PERCENTAGE)/100;
    

        Takehome = Gross - Employee_PF - Employee_ESIC;
       
    
    
    
    
    
        }
       Console.WriteLine("Basic= "+Basic.ToString());
       Console.WriteLine("HRA= "+HRA.ToString());
       Console.WriteLine("Gross= "+Gross.ToString());
       Console.WriteLine("CCA= "+CCA.ToString());
       Console.WriteLine("employee pf="+Employee_PF.ToString());
       Console.WriteLine("employee esic"+ Employee_ESIC.ToString());
       Console.WriteLine("TakeHome "+Takehome.ToString());

    }
    CalcalateSalryStuff(10000);

    }
}