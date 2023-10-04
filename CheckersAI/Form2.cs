using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace CheckersAI
{
    public partial class Form2 : Form
    {
        public System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        public PictureBox prevBox = new PictureBox();
        public PictureBox prevTipBox = new PictureBox();
        public PictureBox comboAttackBox = new PictureBox();
        public const int N = 7;
        public bool moveBiz = true;
        public int attackCount = 0;
        public short blackCount = 0;
        public short whiteCount = 0;
        public int enemyAttackI = 0;
        public int enemyAttackJ = 0;
        public int enemyAttackIDam = 0;
        public int enemyAttackJDam = 0;
        public int enemyAttackIDamWhiteDel = 0;
        public int enemyAttackJDamWhiteDel = 0;
        public short side = 0;
        public int safeI = 0;
        public int safeJ = 0;
        public int DamI = 0;
        public int DamJ = 0;
        public int globalI = 0;
        public int globalJ = 0;
        public int priorI = 0;
        public int priorJ = 0;
        public int attackPoint = 0;
        public int enemyAttackPoint = -15;
        public int generalPoint = -15;
        public PictureBox choosenBox = new PictureBox();
        public Color tipBoxClr = new Color();
        public int maxWhatWhite = 0;
        public bool isAttack = false;
        public int globalIWhite = 0;
        public int globalJWhite = 0;
        public Color validCell = Color.FromArgb(100, 0, 255, 0);
        public Color invalidCell = Color.FromArgb(100,255,0,0);
        public Color moveHistory = Color.FromArgb(100, 128, 128, 128);
        public int chToDam = 0;

        public int[,] RGmap = new int[8, 8]
        {
            {0,2,0,2,0,2,0,2 },
            {2,0,2,0,2,0,2,0 },
            {0,2,0,2,0,2,0,2 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {1,0,1,0,1,0,1,0 },
            {0,1,0,1,0,1,0,1 },
            {1,0,1,0,1,0,1,0 },
        };
        public int[,] map = new int[8, 8]
        {
            {0,2,0,2,0,2,0,2 },
            {2,0,2,0,2,0,2,0 },
            {0,2,0,2,0,2,0,2 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {1,0,1,0,1,0,1,0 },
            {0,1,0,1,0,1,0,1 },
            {1,0,1,0,1,0,1,0 },
        };
        public int[,] RG2ap = new int[8, 8]
       {
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,2 },
            {0,0,0,0,0,0,1,0 },
       };
        public int[] mapPr = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] mapPrX = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] mapPrXx = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] mapPrDam = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] mapPrXDam = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] mapPrXxDam = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public Form2()
        {
            Program.f2 = this;
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Form1 shF1 = new Form1();
            shF1.Show();
            RefreshField();
            this.Controls.Clear();
            RefreshMapImage();
        }
        public int ifc = 0, jfc = 0;

        public int GetEnemyAttackPoint() {
            int currentMaxWhatWhite = 0;
            for (int iu = 7; iu >= 0; iu--)
            {
                for (int ju = 7; ju >= 0; ju--)
                {
                    if (map[iu, ju] == 1 || map[iu, ju] == 11) {
                        globalIWhite = iu;
                        globalJWhite = ju;
                        maxWhatWhite = 0;
                        maxWhatDamWh = 0;
                        if (map[iu, ju] == 1) AttackWaysWhite(iu,ju);
                        if (map[iu, ju] == 11)
                            AttackWaysWhiteDam(iu,ju);
                       
                        if (maxWhatWhite > currentMaxWhatWhite)currentMaxWhatWhite = maxWhatWhite;
                        if (maxWhatDamWh * 10 > currentMaxWhatWhite)currentMaxWhatWhite = maxWhatDamWh*10;
                    }
                }
            }
            return currentMaxWhatWhite/10;
        }
        public int moveForwardIRight = 0;
        public int moveForwardJRight = 0;
        public int moveForwardILeft = 0;
        public int moveForwardJLeft = 0;
        public int moveForwardI = 0;
        public int moveForwardJ = 0;
        public bool chLeft = false;
        public bool chRight = false;
        public bool chDam1 = false; 
        public bool chDam2 = false;
        public bool chDam3 = false;
        public bool chDam4 = false;
        public bool chDam1H = false;
        public bool chDam2H = false;
        public bool chDam3H = false;
        public bool chDam4H = false;
        public bool chDam1HR = false;
        public bool chDam2HR = false;
        public bool chDam3HR = false;
        public bool chDam4HR = false;
        public int moveForwardI1 = 0;
        public int moveForwardJ1 = 0;
        public int moveForwardI2 = 0;
        public int moveForwardJ2 = 0;
        public int moveForwardI3 = 0;
        public int moveForwardJ3 = 0;
        public int moveForwardI4 = 0;
        public int moveForwardJ4 = 0;
        public bool MoveForwardRight(int ti, int tj) {
            chRight = false;
            if (ti + 1 <= 7 && tj + 1 <= 7 && map[ti + 1, tj + 1] == 0)
            {
                moveForwardIRight = ti + 1;
                moveForwardJRight = tj + 1;
                chRight = true;
            }
            return chRight;
        }
        public bool MoveForwardLeft(int ti, int tj)
        {
            chLeft = false;
            if (ti + 1 <= 7 && tj - 1 >= 0 && map[ti + 1, tj - 1] == 0)
            {
                moveForwardILeft = ti + 1;
                moveForwardJLeft = tj - 1;
                chLeft = true;
            }
            return chLeft;
        }
        public bool MoveForwardDam1(int ti, int tj, int qu)
        {
            chDam1 = false;
            if (ti - qu * 1 >= 0 && tj - qu * 1 >= 0 && map[ti - qu * 1, tj - qu * 1] == 0)
            {
                moveForwardI1 = ti - qu * 1;
                moveForwardJ1 = tj - qu * 1;
                if (AttackWaysDam(moveForwardI1, moveForwardJ1) && maxWhatDam > maxDamPossibleAttack)
                {
                    maxDamPossibleAttack = maxWhatDam;
                    cordOfmdpa = moveForwardI1 * 10 + moveForwardJ1;
                }
                chDam1 = true;
                chDam1H = true;
            }
            else chDam1HR = false;
            return chDam1;
        }
        public int maxDamPossibleAttack = 0, cordOfmdpa = 0;
        public bool MoveForwardDam2(int ti, int tj, int qu)
        {
            chDam2 = false;
            if (ti - qu * 1 >= 0 && tj + qu * 1 <= 7 && map[ti - qu * 1, tj + qu * 1] == 0)
            {
                moveForwardI2 = ti - qu * 1;
                moveForwardJ2 = tj + qu * 1;
                if (AttackWaysDam(moveForwardI2, moveForwardJ2) && maxWhatDam > maxDamPossibleAttack)
                {
                    maxDamPossibleAttack = maxWhatDam;
                    cordOfmdpa = moveForwardI2 * 10 + moveForwardJ2;
                }
                 chDam2 = true;
                chDam2H = true;
            }
            else chDam2HR = false;
            return chDam2;
        }
        public bool MoveForwardDam3(int ti, int tj, int qu)
        {
            chDam3 = false;
            if (ti + qu * 1 <= 7 && tj - qu * 1 >= 0 && map[ti + qu * 1, tj - qu * 1] == 0)
            {
                moveForwardI3 = ti + qu * 1;
                moveForwardJ3 = tj - qu * 1;
                if (AttackWaysDam(moveForwardI3, moveForwardJ3) && maxWhatDam > maxDamPossibleAttack)
                {
                    maxDamPossibleAttack = maxWhatDam;
                    cordOfmdpa = moveForwardI3 * 10 + moveForwardJ3;
                }
                chDam3 = true;
                chDam3H = true;
            }
            else chDam3HR = false;
            return chDam3;
        }
        public bool MoveForwardDam4(int ti, int tj, int qu)
        {
            chDam4 = false;
            if (ti + qu * 1 <= 7 && tj + qu * 1 <= 7 && map[ti + qu * 1, tj + qu * 1] == 0)
            {
                moveForwardI4 = ti + qu * 1;
                moveForwardJ4 = tj + qu * 1;
                if (AttackWaysDam(moveForwardI4, moveForwardJ4) && maxWhatDam > maxDamPossibleAttack)
                {
                    maxDamPossibleAttack = maxWhatDam;
                    cordOfmdpa = moveForwardI4 * 10 + moveForwardJ4;
                }
                chDam4 = true;
                chDam4H = true;
            }
            else chDam4HR = false;
            return chDam4;
        }
        public void OutputMapPr(int[] numbers) {
            string str = "";
            for (int q = 0; q < 12; q++) str += numbers[q].ToString() + " ";
            MessageBox.Show(str);
        }
        public void OutputMapPr2(int[,] numbers)
        {
            string str = "";
            for (int q = 0; q < 8; q++) 
            for (int k = 0; k < 8; k++) {
                    str += numbers[q,k].ToString() + " " ;
                    if (k == 7) str += Environment.NewLine;
                }
            MessageBox.Show(str);
        }
        int stageWayGlob = 0;
        public int FindWhiteCheck(int curI, int curJ, int mI, int mJ) {
            if (mI < curI && mJ < curJ) 
            {
                for (int ip = 0; ip < 6; ip++) if (map[curI - ip, curJ - ip] == 1 || map[curI - ip, curJ - ip] == 11)
                    {
                        stageWayGlob = 1;
                        return (curI - ip) * 10 + (curJ - ip);
                    }

            }
            if (mI > curI && mJ < curJ)
            {
                for (int ip = 0; ip < 6; ip++) if (map[curI + ip, curJ - ip] == 1 || map[curI + ip, curJ - ip] == 11) 
                {
                    stageWayGlob = 3;
                    return (curI + ip) * 10 + (curJ - ip);
                }

            }
            if (mI < curI && mJ > curJ)
            {
                for (int ip = 0; ip < 6; ip++) if (map[curI - ip, curJ + ip] == 1 || map[curI - ip, curJ + ip] == 11)
                {
                    stageWayGlob = 2;
                    return (curI - ip) * 10 + (curJ + ip);
                }
            }
            if (mI > curI && mJ > curJ)
            {
                for (int ip = 0; ip < 6; ip++) if (map[curI + ip, curJ + ip] == 1 || map[curI + ip, curJ + ip] == 11)
                {
                    stageWayGlob = 4;
                    return (curI + ip) * 10 + (curJ + ip);
                }
            }
            return mI* 10 + mJ;
        }
        public async Task GenerateMoveEnemyAI() {
            int ko2 = 0, ko1 = 0;
            int maxWhatStatic = 0;
            int maxWhatStaticDam = 0;
            maxWhat = 0;
            willAttackedWhiteCheckerCount = 0;
            stageWay = 0;
            isEndOfAttackWay = false;
            mapPrCount = 0;
            currentpos = 0;
            attackCounter = 0;
            maxWhatDam = 0;
            currentPosDam = 0;
            mapPrCountDam = 0;
            isEndOfAttcakDam = false;
            int mode = 0;
            generalPoint = -15;
            int tempChe = 0;
            for (int i = 7; i >= 0; i--) 
            {
                for (int j = 7; j >= 0; j--)
                {
                    ifc = i;
                    jfc = j;
                    int prem = 0;
                    chToDam = 0;
                    attackCount = 0;
                    if (map[i, j] == 2 && !moveBiz)
                    {
                        for (int q = 0; q < 12; q++)
                        {
                            mapPr[q] = 0;
                            moble[q] = 0;
                            moble2[q] = 0;
                        }
                        gI = i;
                        gJ = j;
                        globalI = i;
                        globalJ = j;
                        blackCount++;
                        maxWhat = 0;
                        AttackWays(i, j);
                        //map[7, 4] = 22;
                        //AttackWaysDam(7, 4);



                        MoveForwardRight(i, j);
                        MoveForwardLeft(i, j);
                        bool kGran = false;
                        if (chToDam != 0)
                        {
                            for (int q = maxWhatDam - 1, qq1 = 0; q >= 0; q--, qq1++) moble[qq1] = moble2[q];
                            for (int q = 0, q1 = 0; q < 12; q++)
                            {
                                if (moble[q] == 0 && !kGran) kGran = true;
                                if (kGran) { mapPrX[q] = mapPr[q1]; q1++; }
                                else { mapPrX[q] = moble[q]; }
                            }
                        }
                        else
                            for (int q = 0; q < 12; q++) mapPrX[q] = mapPr[q];
                        if (maxWhat != 0)
                        {
                            int[] findedWhiteCheckers = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            int currentI = i, currentJ = j;
                            mapPrX[maxWhat / 10] = i * 10 + j;
                            int whPls = 0;
                            try
                            {
                                for (int il = maxWhat / 10 - 1; il >= 0; il--)
                                {
                                    if (mapPrX[il] != 0)
                                    {
                                        whPls = FindWhiteCheck(currentI, currentJ, mapPrX[il] / 10, mapPrX[il] % 10);
                                        findedWhiteCheckers[il] = whPls;
                                        map[currentI, currentJ] = 0;
                                        map[whPls / 10, whPls % 10] = 0;
                                        currentI = mapPrX[il] / 10;
                                        currentJ = mapPrX[il] % 10;


                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());

                                throw;
                            }

                            currentI = i;
                            currentJ = j;

                            if (chToDam != 0) prem = prem + 1;
                            enemyAttackPoint = GetEnemyAttackPoint();
                            try
                            {
                                for (int il = 0; il <= maxWhat / 10 - 1; il++)
                                {
                                    if (findedWhiteCheckers[il] != 0)
                                    {
                                        map[findedWhiteCheckers[il] / 10, findedWhiteCheckers[il] % 10] = 1;

                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                                throw;
                            }

                            map[currentI, currentJ] = 2;
                            attackPoint = maxWhat / 10 + prem;
                            if (attackPoint - enemyAttackPoint + 15 >= generalPoint)
                            {
                                for (int q = 0; q < 12; q++) mapPrXx[q] = mapPrX[q];
                                generalPoint = attackPoint - enemyAttackPoint + 15;
                                priorI = i;
                                priorJ = j;
                                maxWhatStatic = maxWhat;
                                mode = 1;
                            }
                        }
                        if (chLeft || chRight)
                        {
                            int tempL = -15;
                            int tempR = -15;
                            int pary = 0;
                            if (chRight)
                            {
                                map[i, j] = 0;
                                map[moveForwardIRight, moveForwardJRight] = 2;
                                tempR = -GetEnemyAttackPoint();
                                map[i, j] = 2;
                                map[moveForwardIRight, moveForwardJRight] = 0;
                                if (moveForwardIRight == 7) tempR++;
                            }
                            if (chLeft)
                            {
                                map[i, j] = 0;
                                map[moveForwardILeft, moveForwardJLeft] = 2;
                                tempL = -GetEnemyAttackPoint();
                                map[i, j] = 2;
                                map[moveForwardILeft, moveForwardJLeft] = 0;
                                if (moveForwardILeft == 7) tempL++;
                            }
                            if (tempL == tempR)
                            {
                                Random rnd = new Random();
                                pary = rnd.Next(1, 3);
                            }
                            if (tempL >= generalPoint && (tempL > tempR || pary == 1) && chLeft)
                            {
                                generalPoint = tempL;
                                priorI = i;
                                priorJ = j;
                                moveForwardI = moveForwardILeft;
                                moveForwardJ = moveForwardJLeft;
                                mode = 3;
                            }
                            if (tempR >= generalPoint && (tempR > tempL || pary == 2) && chRight)
                            {
                                generalPoint = tempR;
                                priorI = i;
                                priorJ = j;
                                moveForwardI = moveForwardIRight;
                                moveForwardJ = moveForwardJRight;
                                mode = 3;
                            }
                        }
                    }
                    else if (!moveBiz && map[i, j] == 22)
                    {
                        blackCount++;
                        chDam1H = false;
                        chDam2H = false;
                        chDam3H = false;
                        chDam4H = false;
                        maxWhatDam = 0;
                        AttackWaysDam(i, j);
                        int fz = 0;

                        for (int q = maxWhatDam - 1, q1 = 0; q >= 0; q--, q1++) mapPrXDam[q1] = moble2[q];
                        if (maxWhatDam != 0)
                        {
                            int currentI = i, currentJ = j;
                            mapPrXDam[maxWhatDam] = i * 10 + j;
                            int whChPos = 0;
                            //for (int il = maxWhatDam - 1; il >= 0; il--)
                            //{
                            //    if (mapPrXDam[il] != 0)
                            //    {
                            //        //if (mapPrXDam[il] / 10 - currentI > 0) ko1 = 1;
                            //        //else ko1 = -1;
                            //        //if (mapPrXDam[il] % 10 - currentJ > 0) ko2 = 1;
                            //        //else ko2 = -1;
                            //        map[mapPrXDam[il] / 10, mapPrXDam[il] % 10] = 22;
                            //        whChPos = FindWhiteCheck(currentI,currentJ,mapPrXDam[il] / 10, mapPrXDam[il] % 10);
                            //        map[currentI, currentJ] = 0;
                            //        tempChe = map[whChPos/10,whChPos%10];
                            //        map[whChPos / 10, whChPos % 10] = 0;
                            //        currentI = mapPrXDam[il] / 10;
                            //        currentJ = mapPrXDam[il] % 10;
                            //    }
                            //}
                            enemyAttackPoint = GetEnemyAttackPoint();
                            //for (int il = 1; il <= maxWhatDam; il++)
                            //{
                            //    if (mapPrXDam[il] != 0)
                            //    {
                            //        whChPos = FindWhiteCheck(currentI, currentJ, mapPrXDam[il] / 10, mapPrXDam[il] % 10);
                            //        //if (mapPrXDam[il] / 10 - currentI > 0) ko1 = 1;
                            //        //else ko1 = -1;
                            //        //if (mapPrXDam[il] % 10 - currentJ > 0) ko2 = 1;
                            //        //else ko2 = -1;
                            //        map[mapPrXDam[il] / 10, mapPrXDam[il] % 10] = 22;
                            //        map[currentI, currentJ] = 0;
                            //        map[whChPos/10,whChPos%10] = tempChe;
                            //        currentI = mapPrXDam[il] / 10;
                            //        currentJ = mapPrXDam[il] % 10;
                            //    }

                            //}
                            attackPoint = maxWhatDam;
                            if (attackPoint - enemyAttackPoint + 15 >= generalPoint)
                            {
                                for (int q = 0; q < 12; q++) mapPrXxDam[q] = mapPrXDam[q];
                                generalPoint = attackPoint - enemyAttackPoint + 15;
                                priorI = i;
                                priorJ = j;
                                maxWhatStaticDam = maxWhatDam;
                                mode = 4;
                            }
                        }
                        else { 
                        for (int chcker = 1; chcker < 7; chcker++)
                        {
                            MoveForwardDam1(i, j, chcker);
                            MoveForwardDam2(i, j, chcker);
                            MoveForwardDam3(i, j, chcker);
                            MoveForwardDam4(i, j, chcker);
                        }
                        chDam1HR = true;
                        chDam2HR = true;
                        chDam3HR = true;
                        chDam4HR = true;
                        if (chDam1H || chDam2H || chDam3H || chDam4H)
                        {
                            int temp1 = -15;
                            int temp2 = -15;
                            int temp3 = -15;
                            int temp4 = -15;
                            int pary = 0;
                            for (int chcker = 1; chcker < 7; chcker++)
                            {
                                MoveForwardDam1(i, j, chcker);
                                MoveForwardDam2(i, j, chcker);
                                MoveForwardDam3(i, j, chcker);
                                MoveForwardDam4(i, j, chcker);
                                if (chDam1 && chDam1HR)
                                {
                                    map[i, j] = 0;
                                    map[moveForwardI1, moveForwardJ1] = 22;
                                    temp1 = -GetEnemyAttackPoint();
                                    map[i, j] = 22;
                                    map[moveForwardI1, moveForwardJ1] = 0;
                                }
                                if (chDam2 && chDam2HR)
                                {
                                    map[i, j] = 0;
                                    map[moveForwardI2, moveForwardJ2] = 22;
                                    temp2 = -GetEnemyAttackPoint();
                                    map[i, j] = 22;
                                    map[moveForwardI2, moveForwardJ2] = 0;
                                }
                                if (chDam3 && chDam3HR)
                                {
                                    map[i, j] = 0;
                                    map[moveForwardI3, moveForwardJ3] = 22;
                                    temp3 = -GetEnemyAttackPoint();
                                    map[i, j] = 22;
                                    map[moveForwardI3, moveForwardJ3] = 0;
                                }
                                if (chDam4 && chDam4HR)
                                {
                                    map[i, j] = 0;
                                    map[moveForwardI4, moveForwardJ4] = 22;
                                    temp4 = -GetEnemyAttackPoint();
                                    map[i, j] = 22;
                                    map[moveForwardI4, moveForwardJ4] = 0;
                                }
                                if (temp1 == temp2 && temp3 == temp4 && temp2 == temp3)
                                {
                                    Random rnd = new Random();
                                    pary = rnd.Next(1, 5);
                                }
                                if (temp1 >= generalPoint/* && (temp1 > temp2 && temp1 > temp3 && temp1 > temp4 || pary == 1)*/ && chDam1HR)
                                {
                                    generalPoint = temp1;
                                    priorI = i;
                                    priorJ = j;
                                    moveForwardI = moveForwardI1;
                                    moveForwardJ = moveForwardJ1;
                                    mode = 5;
                                }
                                if (temp2 >= generalPoint/* && (temp2 > temp1 && temp2 > temp3 && temp2 > temp4 || pary == 2)*/ && chDam2HR)
                                {
                                    generalPoint = temp2;
                                    priorI = i;
                                    priorJ = j;
                                    moveForwardI = moveForwardI2;
                                    moveForwardJ = moveForwardJ2;
                                    mode = 5;
                                }
                                if (temp3 >= generalPoint /*&& (temp3 > temp2 && temp3 > temp1 && temp3 > temp4 || pary == 3)*/ && chDam3HR)
                                {
                                    generalPoint = temp3;
                                    priorI = i;
                                    priorJ = j;
                                    moveForwardI = moveForwardI3;
                                    moveForwardJ = moveForwardJ3;
                                    mode = 5;
                                }
                                if (temp4 >= generalPoint /*&& (temp4 > temp2 && temp4 > temp3 && temp4 > temp1 || pary == 4)*/ && chDam4HR)
                                {
                                    generalPoint = temp4;
                                    priorI = i;
                                    priorJ = j;
                                    moveForwardI = moveForwardI4;
                                    moveForwardJ = moveForwardJ4;
                                    mode = 5;
                                }
                                if (cordOfmdpa != 0 && maxDamPossibleAttack > generalPoint)
                                {
                                    generalPoint = maxWhatDam;
                                    moveForwardI = cordOfmdpa / 10;
                                    moveForwardJ = cordOfmdpa % 10;
                                    priorI = i;
                                    priorJ = j;
                                    mode = 5;
                                    cordOfmdpa = 0;
                                    maxWhatDam = 0;
                                    maxDamPossibleAttack = 0;
                                }
                            }
                        }
                    }
                    }
                }
            }
            switch (mode)
            {
                case 1:
                    int currentI = priorI, currentJ = priorJ;
                    bool mdxp = false;
                    int whPl = 0;
                    for (int il = maxWhatStatic / 10 - 1; il >= 0; il--)
                    {
                        
                        if (mapPrXx[il] != 0)
                        {
                            try
                            {
                                if (mdxp) map[mapPrXx[il] / 10, mapPrXx[il] % 10] = 22;
                                else map[mapPrXx[il] / 10, mapPrXx[il] % 10] = 2;
                                whPl = FindWhiteCheck(currentI, currentJ, mapPrXx[il] / 10, mapPrXx[il] % 10);
                               //MessageBox.Show(whPl.ToString() +" "+ currentI + " "+currentJ + mapPrXx[il] / 10 + " "+ mapPrXx[il] % 10);
                                map[currentI, currentJ] = 0;
                                map[whPl / 10, whPl % 10] = 0;
                                currentI = mapPrXx[il] / 10;
                                currentJ = mapPrXx[il] % 10;
                                if (il == 0 && mdxp) FindHappyEnd(currentI,currentJ);
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            System.IO.Stream resourceStream1 =
                                assembly.GetManifestResourceStream(@"CheckersAI.game_piece_movement_09.wav");
                            SoundPlayer sndpl = new SoundPlayer(resourceStream1);
                            await Task.Delay(500);
                            if (currentI == 7) mdxp = true;
                            this.Controls.Clear();
                            RefreshMapImage();
                            sndpl.Play();
                        }
                       
                    }
                    if (currentI == 7) map[currentI, currentJ] = 22;
                    moveBiz = true;
                    break;
                case 2:
                    map[priorI, priorJ] = 0;
                    map[DamI, DamJ] = 22;
                    moveBiz = true;
                    break;
                case 3:
                    
                    System.IO.Stream resourceStream =
                        assembly.GetManifestResourceStream(@"CheckersAI.movement_01 (online-audio-converter.com).wav");
                    SoundPlayer player = new SoundPlayer(resourceStream);
                    
                    map[priorI, priorJ] = 0;
                    if(moveForwardI == 7)
                    map[moveForwardI, moveForwardJ] = 22;
                    else 
                    map[moveForwardI, moveForwardJ] = 2;
                    moveBiz = true;
                    await Task.Delay(500);
                    player.Play();
                    break;
                case 4:
                    int currentIDam = priorI, currentJDam = priorJ;
                    int wchpos = 0;
                    for (int il = maxWhatStaticDam - 1; il >= 0; il--)
                    {
                        if (mapPrXxDam[il] != 0)
                        {
                            wchpos = FindWhiteCheck(currentIDam, currentJDam, mapPrXxDam[il] / 10, mapPrXxDam[il] % 10);
                            //if (mapPrXxDam[il] / 10 - currentIDam > 0) ko1 = 1;
                            //else ko1 = -1;
                            //if (mapPrXxDam[il] % 10 - currentJDam > 0) ko2 = 1;
                            //else ko2 = -1;
                            map[mapPrXxDam[il] / 10, mapPrXxDam[il] % 10] = 22;
                            map[currentIDam, currentJDam] = 0;
                            map[wchpos/10,wchpos%10] = 0;
                            currentIDam = mapPrXxDam[il] / 10;
                            currentJDam = mapPrXxDam[il] % 10;
                            System.IO.Stream resourceStream1 =
                                assembly.GetManifestResourceStream(@"CheckersAI.game_piece_movement_09.wav");
                            SoundPlayer sndpl = new SoundPlayer(resourceStream1);
                            await Task.Delay(500);
                            this.Controls.Clear();
                            RefreshMapImage();
                            sndpl.Play();
                        }
                    }
                    moveBiz = true;
                    break;
                case 5:
                    System.IO.Stream resourceStream4 =
                        assembly.GetManifestResourceStream(@"CheckersAI.movement_01 (online-audio-converter.com).wav");
                    SoundPlayer player4 = new SoundPlayer(resourceStream4);
                    await Task.Delay(500);
                    map[priorI, priorJ] = 0;
                    map[moveForwardI, moveForwardJ] = 22;
                    moveBiz = true;
                    player4.Play();
                    break;
                default:
                    System.IO.Stream resourceStream2 =
                        assembly.GetManifestResourceStream(@"CheckersAI.Victory.wav");
                    SoundPlayer player2 = new SoundPlayer(resourceStream2);
                    moveBiz = true;
                    player2.Play(); 
                    MessageBox.Show("Противник сдается");
                    break;
            }
            
            this.Controls.Clear();
            RefreshMapImage();
            WhiteCountCheckers();

        }

        public void FindHappyEnd(int ei, int ej) {
            //int maxDanger = 0;
            //int shag = 0;
            //if (stageWayGlob == 1) while(ei-shag>=0 && ej - shag >= 0)
            //    {
            //        if(map[])

            //    }
                
            
            
        }

        public int attackCounter = 0;
        public int maxWhatDam = 0;
        public int currentPosDam = 0;
        public int mapPrCountDam = 0;
        public bool isEndOfAttcakDam= false;
        public int[,] moveHistoryTable = new int[100,10];
        public int[] moble = new int[20];
        public int[] moble2 = new int[20];


        public int ifh = 0, jfh = 0, ifm =0;
        public bool AttackWaysDam(int ti, int tj) {
            bool mdx = false , mdx1 = false;
            if (ti >= 0 && tj >= 0 )
                for (int attackedPostI = ti - 1, attackedPostJ = tj - 1 ;; attackedPostI--, attackedPostJ--) {
                    if (attackedPostI - 1 < 0 && attackedPostJ - 1 < 0 || attackedPostI - 1 < 0 || attackedPostJ - 1 < 0) break;
                    if (map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) break;
                    if ((map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) && (map[attackedPostI + 1, attackedPostJ + 1] <= 0 
                        || map[attackedPostI + 1, attackedPostJ + 1] == 22) && map[attackedPostI - 1, attackedPostJ - 1] <= 0) 
                    {
                        stageWay = 1;
                        attackCounter++;
                        
                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 1) map[attackedPostI, attackedPostJ] = -1;
                        if (map[attackedPostI, attackedPostJ] == 11) map[attackedPostI, attackedPostJ] = -11;
                        int nextAtkI = attackedPostI - 1, nextAtkJ = attackedPostJ - 1;
                        for (; nextAtkI >= 0 && nextAtkJ >= 0; nextAtkI--, nextAtkJ--, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDam)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDam = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                            if (isEndOfAttcakDam && currentPosDam == attackCounter)
                            {
                            if (nextAtkI < 0 || nextAtkJ < 0) { nextAtkI++; nextAtkJ++; }
                                mapPrDam[mapPrCountDam] = 10 * nextAtkI + nextAtkJ;//++
                                mapPrCountDam++;
                                currentPosDam--;
                            }
                            if (maxWhatDam - mapPrCountDam == 0)
                            {
                                isEndOfAttcakDam = false;
                                mapPrCountDam = 0;
                            }
                        
                        if (map[attackedPostI, attackedPostJ] == -1) map[attackedPostI, attackedPostJ] = 1;
                        if (map[attackedPostI, attackedPostJ] == -11) map[attackedPostI, attackedPostJ] = 11;
                        attackCounter--;
                        
                    }
                }
            if (ti >= 0 && tj <= 7)
             
                for (int attackedPostI = ti - 1, attackedPostJ = tj + 1; ; attackedPostI--, attackedPostJ++)
                {
                    if (attackedPostI - 1 < 0 && attackedPostJ + 1 > 7 || attackedPostI - 1 < 0 || attackedPostJ + 1 > 7) break;
                    if (map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) break;
                    if ((map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) && map[attackedPostI - 1, attackedPostJ + 1] <= 0
                        && (map[attackedPostI + 1, attackedPostJ - 1] <= 0 || map[attackedPostI + 1, attackedPostJ - 1] == 22))
                    {
                        
                        stageWay = 2;
                        attackCounter++;
                        
                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 1) map[attackedPostI, attackedPostJ] = -1;
                        if (map[attackedPostI, attackedPostJ] == 11) map[attackedPostI, attackedPostJ] = -11;
                        int nextAtkI = attackedPostI - 1, nextAtkJ = attackedPostJ + 1;
                        for (; nextAtkI >= 0 && nextAtkJ <= 7; nextAtkI--, nextAtkJ++, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDam)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDam = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI < 0 || nextAtkJ > 7) { nextAtkI++; nextAtkJ--; }
                            mapPrDam[mapPrCountDam] = 10 * nextAtkI + nextAtkJ ;//+-
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDam - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }
                        if (map[attackedPostI, attackedPostJ] == -1) map[attackedPostI, attackedPostJ] = 1;
                        if (map[attackedPostI, attackedPostJ] == -11) map[attackedPostI, attackedPostJ] = 11;
                        attackCounter--;
                        
                    }
                }
            if (ti <= 7 && tj >= 0)
                for (int attackedPostI = ti + 1, attackedPostJ = tj - 1; ; attackedPostI++, attackedPostJ--)
                {
                    
                    if (attackedPostI + 1 > 7 && attackedPostJ - 1 < 0 || attackedPostI + 1 > 7  || attackedPostJ - 1 < 0 ) break;
                    if (map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) break;
                    if ((map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) && (map[attackedPostI + 1, attackedPostJ - 1] == 0 
                        || map[attackedPostI + 1, attackedPostJ - 1] < 0)
                        && (map[attackedPostI - 1, attackedPostJ + 1] == 0 || map[attackedPostI - 1, attackedPostJ + 1] == 22 || map[attackedPostI - 1, attackedPostJ + 1] < 0))
                    {
                        stageWay = 3;
                        attackCounter++;
                       
                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 1) map[attackedPostI, attackedPostJ] = -1;
                        if (map[attackedPostI, attackedPostJ] == 11) map[attackedPostI, attackedPostJ] = -11;
                        int nextAtkI = attackedPostI + 1, nextAtkJ = attackedPostJ - 1;
                        for (; nextAtkI <= 7 && nextAtkJ >= 0; nextAtkI++, nextAtkJ--, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDam)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDam = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI > 7 || nextAtkJ < 0) { nextAtkI--; nextAtkJ++; }
                            mapPrDam[mapPrCountDam] = 10 * (nextAtkI ) + (nextAtkJ );//-+
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDam - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }
                        if (map[attackedPostI, attackedPostJ] == -1) map[attackedPostI, attackedPostJ] = 1;
                        if (map[attackedPostI, attackedPostJ] == -11) map[attackedPostI, attackedPostJ] = 11;
                        attackCounter--;
                        
                    }
                }
            if (ti <= 7 && tj <= 7)
                for (int attackedPostI = ti + 1, attackedPostJ = tj + 1; ; attackedPostI++, attackedPostJ++)
                {
                    if (attackedPostI + 1 > 7 && attackedPostJ + 1 > 7 || attackedPostI + 1 > 7 || attackedPostJ + 1> 7) break;
                    if (map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) break;
                    if ((map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) && map[attackedPostI + 1, attackedPostJ + 1] <= 0 
                        && (map[attackedPostI -1, attackedPostJ - 1] <= 0 || map[attackedPostI - 1, attackedPostJ - 1] == 22))
                    {
                        stageWay = 4;
                    
                        attackCounter++;
                       
                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 1) map[attackedPostI, attackedPostJ] = -1;
                        if (map[attackedPostI, attackedPostJ] == 11) map[attackedPostI, attackedPostJ] = -11;
                        int nextAtkI = attackedPostI + 1, nextAtkJ = attackedPostJ + 1;
                        for (; nextAtkI <= 7 && nextAtkJ <= 7; nextAtkI++, nextAtkJ++, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDam)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDam = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI > 7 || nextAtkJ > 7) { nextAtkI--; nextAtkJ--; }
                            mapPrDam[mapPrCountDam] = 10 * (nextAtkI) + (nextAtkJ);//++
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDam - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }
                        if (map[attackedPostI, attackedPostJ] == -1) map[attackedPostI, attackedPostJ] = 1;
                        if (map[attackedPostI, attackedPostJ] == -11) map[attackedPostI, attackedPostJ] = 11;
                        attackCounter--;
                       
                    }
                }
            return mdx;
        }
        public int willAttackedWhiteCheckerCount = 0;
        public byte stageWay = 0;
        public bool isEndOfAttackWay = false;
        public int maxWhat =0;
        public int mapPrCount = 0;
        public int currentpos = 0;
        public bool AttackWays(int ti, int tj) {
            bool mdx = false;
            if (ti - 2 >= 0 && tj - 2 >= 0 && (map[ti - 1, tj - 1] == 1 || map[ti - 1, tj - 1] == 11) && (map[ti - 2, tj - 2] % 10 == 0 || (ti - 2 == globalI && tj - 2 == globalJ)) && stageWay != 4)
            {
                stageWay = 1;
                willAttackedWhiteCheckerCount++;
                mdx = true;
                enemyAttackJ = tj - 2;
                enemyAttackI = ti - 2;
                if (map[ti - 1, tj - 1] == 1) map[ti - 1, tj - 1] = -1;
                if (map[ti - 1, tj - 1] == 11) map[ti - 1, tj - 1] = -11;
                if (enemyAttackI == 7)
                {
                    chToDam = enemyAttackI * 10 + enemyAttackJ;
                    attackCounter = 0;
                    maxWhatDam = 0;
                    currentPosDam = 0;
                    mapPrCountDam = 0;
                    isEndOfAttcakDam = false;
                    ifm = 0;
                    AttackWaysDam(enemyAttackI, enemyAttackJ);
                    stageWay = 1;
                    isEndOfAttackWay = true;
                    maxWhat = (maxWhatDam + willAttackedWhiteCheckerCount) * 10;
                    currentpos = willAttackedWhiteCheckerCount;
                    mapPrCount = 0;

                    // return false;
                }
                else
                {
                    if (!AttackWays(enemyAttackI, enemyAttackJ))
                    {
                        if (willAttackedWhiteCheckerCount * 10 > maxWhat)
                        {
                            currentpos = willAttackedWhiteCheckerCount;
                            maxWhat = willAttackedWhiteCheckerCount * 10;
                            isEndOfAttackWay = true;
                            mapPrCount = 0;
                        }
                    }
                }
                if (isEndOfAttackWay && currentpos==willAttackedWhiteCheckerCount) {
                    mapPr[mapPrCount] = 10 * (ti - 2) + tj - 2;
                    mapPrCount++;
                    currentpos--;
                }
                if (maxWhat - 10 * mapPrCount == 0)
                {
                    isEndOfAttackWay = false;
                    mapPrCount = 0;
                }
                if (map[ti - 1, tj - 1] == -1) map[ti - 1, tj - 1] = 1;
                if (map[ti - 1, tj - 1] == -11) map[ti - 1, tj - 1] = 11;
                willAttackedWhiteCheckerCount--;
            }
            if (ti - 2 >= 0 && tj + 2 <= 7 && (map[ti - 1, tj + 1] == 1 || map[ti - 1, tj + 1] == 11) && (map[ti - 2, tj + 2] % 10 == 0 || (ti - 2 == globalI && tj + 2 == globalJ)) && stageWay != 3)
            {
                stageWay = 2;
                willAttackedWhiteCheckerCount++;
                mdx = true;
                enemyAttackJ = tj + 2;
                enemyAttackI = ti - 2;
                if (map[ti - 1, tj + 1] == 1) map[ti - 1, tj + 1] = -1;
                if (map[ti - 1, tj + 1] == 11) map[ti - 1, tj + 1] = -11;
                if (enemyAttackI == 7)
                {
                    chToDam = enemyAttackI * 10 + enemyAttackJ;
                    attackCounter = 0;
                    maxWhatDam = 0;
                    currentPosDam = 0;
                    mapPrCountDam = 0;
                    isEndOfAttcakDam = false;
                    ifm = 0;
                    AttackWaysDam(enemyAttackI, enemyAttackJ);
                    stageWay = 2;
                    isEndOfAttackWay = true;
                    maxWhat = (maxWhatDam + willAttackedWhiteCheckerCount) * 10;
                    currentpos = willAttackedWhiteCheckerCount;
                    mapPrCount = 0;

                    // return false;
                }
                else
                {
                    if (!AttackWays(enemyAttackI, enemyAttackJ))
                    {
                        if (willAttackedWhiteCheckerCount * 10 > maxWhat)
                        {
                            currentpos = willAttackedWhiteCheckerCount;
                            maxWhat = willAttackedWhiteCheckerCount * 10;
                            isEndOfAttackWay = true;
                            mapPrCount = 0;
                        }
                    }
                }
                if (isEndOfAttackWay && currentpos == willAttackedWhiteCheckerCount)
                {
                    mapPr[mapPrCount] = 10 * (ti - 2) + tj + 2;
                    mapPrCount++;
                    currentpos--;
                }
                if (maxWhat - 10 * mapPrCount == 0)
                {
                    isEndOfAttackWay = false;
                    mapPrCount = 0;
                }
                if (map[ti - 1, tj + 1] == -1) map[ti - 1, tj + 1] = 1;
                if (map[ti - 1, tj + 1] == -11) map[ti - 1, tj + 1] = 11;
                willAttackedWhiteCheckerCount--;
            }
            if (ti + 2 <= 7 && tj - 2 >= 0 && (map[ti + 1, tj - 1] == 1 || map[ti + 1, tj - 1] == 11) && (map[ti + 2, tj - 2] % 10 == 0 || (ti + 2 == globalI && tj - 2 == globalJ)) && stageWay != 2) 
            {
                stageWay = 3;
                willAttackedWhiteCheckerCount++;
                mdx = true;
                enemyAttackJ = tj - 2;
                enemyAttackI = ti + 2;
                if (map[ti + 1, tj - 1] == 1) map[ti + 1, tj - 1] = -1;
                if (map[ti + 1, tj - 1] == 11) map[ti + 1, tj - 1] = -11;
                if (enemyAttackI == 7)
                {
                    chToDam = enemyAttackI * 10 + enemyAttackJ;
                    attackCounter = 0;
                    maxWhatDam = 0;
                    currentPosDam = 0;
                    mapPrCountDam = 0;
                    isEndOfAttcakDam = false;
                    ifm = 0;
                    AttackWaysDam(enemyAttackI, enemyAttackJ);
                    stageWay = 3;
                    isEndOfAttackWay = true;
                    maxWhat = (maxWhatDam + willAttackedWhiteCheckerCount) * 10;
                    currentpos = willAttackedWhiteCheckerCount;
                    mapPrCount = 0;
                   // return false;
                }
                else
                {
                    if (!AttackWays(enemyAttackI, enemyAttackJ))
                    {
                        if (willAttackedWhiteCheckerCount * 10 > maxWhat)
                        {
                            currentpos = willAttackedWhiteCheckerCount;
                            maxWhat = willAttackedWhiteCheckerCount * 10;
                            isEndOfAttackWay = true;
                            mapPrCount = 0;
                        }
                    }
                }
                if (isEndOfAttackWay && currentpos == willAttackedWhiteCheckerCount)
                {
                    mapPr[mapPrCount] = 10 * (ti + 2) + tj - 2;
                    mapPrCount++;
                    currentpos--;
                }
                if (maxWhat - 10 * mapPrCount == 0)
                {
                    isEndOfAttackWay = false;
                    mapPrCount = 0;
                }
                if (map[ti + 1, tj - 1] == -1) map[ti + 1, tj - 1] = 1;
                if (map[ti + 1, tj - 1] == -11) map[ti + 1, tj - 1] = 11;
                willAttackedWhiteCheckerCount--;
            }
            if (ti + 2 <= 7 && tj + 2 <= 7 && (map[ti + 1, tj + 1] == 1 || map[ti + 1, tj + 1] == 11) && (map[ti + 2, tj + 2] % 10 == 0 || (ti + 2 == globalI && tj + 2 == globalJ
                && map[globalI, globalJ] == 2)) && stageWay != 1)
            {
                stageWay = 4;
                willAttackedWhiteCheckerCount++;
                mdx = true;
                enemyAttackJ = tj + 2;
                enemyAttackI = ti + 2;
                if (map[ti + 1, tj + 1] == 1) map[ti + 1, tj + 1] = -1;
                if (map[ti + 1, tj + 1] == 11) map[ti + 1, tj + 1] = -11;
                if (enemyAttackI == 7)
                {
                    chToDam = enemyAttackI * 10 + enemyAttackJ;
                    attackCounter = 0;
                    maxWhatDam = 0;
                    currentPosDam = 0;
                    mapPrCountDam = 0;
                    isEndOfAttcakDam = false;
                    ifm = 0;
                    AttackWaysDam(enemyAttackI, enemyAttackJ);
                    stageWay = 4;
                    isEndOfAttackWay = true;
                    maxWhat = (maxWhatDam + willAttackedWhiteCheckerCount) * 10;
                    currentpos = willAttackedWhiteCheckerCount;
                    mapPrCount = 0;
                    // return false;
                }
                else
                {
                    if (!AttackWays(enemyAttackI, enemyAttackJ))
                    {
                        if (willAttackedWhiteCheckerCount * 10 > maxWhat)
                        {
                            currentpos = willAttackedWhiteCheckerCount;
                            maxWhat = willAttackedWhiteCheckerCount * 10;
                            isEndOfAttackWay = true;
                            mapPrCount = 0;
                        }
                    }
                }
                if (isEndOfAttackWay && currentpos == willAttackedWhiteCheckerCount)
                {
                    mapPr[mapPrCount] = 10 * (ti+2) + tj+2;
                    mapPrCount++;
                    currentpos--;
                }
                if (maxWhat - 10 * mapPrCount == 0)
                {
                    isEndOfAttackWay = false;
                    mapPrCount = 0;
                }
                if (map[ti + 1, tj + 1] == -1) map[ti + 1, tj + 1] = 1;
                if (map[ti + 1, tj + 1] == -11) map[ti + 1, tj + 1] = 11;
                willAttackedWhiteCheckerCount--;
            }
            stageWay = 0;
            return mdx;
        }
        public int gI = 0;
        public int gJ = 0;
        public int willAttackedWhiteCheckerCountWhite = 0;
        public int maxWhatDamWh = 0;
        public bool AttackWaysWhite(int ti, int tj)
        {
            bool mdx = false;
            if (ti - 2 >= 0 && tj - 2 >= 0 && (map[ti - 1, tj - 1] == 2 || map[ti - 1, tj - 1] == 22) && (map[ti - 2, tj - 2] % 10 == 0 || (ti - 2 == globalIWhite && tj - 2 == globalJWhite
                && map[globalI, globalJ] == 2)) && stageWay != 4)
            {
                stageWay = 1;
                willAttackedWhiteCheckerCountWhite++;
                mdx = true;
                enemyAttackJ = tj - 2;
                enemyAttackI = ti - 2;
                if (map[ti - 1, tj - 1] == 2) map[ti - 1, tj - 1] = -2;
                if (map[ti - 1, tj - 1] == 22) map[ti - 1, tj - 1] = -22;
                if (!AttackWaysWhite(enemyAttackI, enemyAttackJ))
                {
                    if (willAttackedWhiteCheckerCountWhite * 10 > maxWhatWhite)
                    {
                        maxWhatWhite = willAttackedWhiteCheckerCountWhite * 10;
                    }
                }
                if (map[ti - 1, tj - 1] == -2) map[ti - 1, tj - 1] = 2;
                if (map[ti - 1, tj - 1] == -22) map[ti - 1, tj - 1] = 22;
                willAttackedWhiteCheckerCountWhite--;
            }
            if (ti - 2 >= 0 && tj + 2 <= 7 && (map[ti - 1, tj + 1] == 2 || map[ti - 1, tj + 1] == 22) && (map[ti - 2, tj + 2] % 10 == 0 || (ti - 2 == globalIWhite && tj + 2 == globalJWhite
                && map[globalI, globalJ] == 2)) && stageWay != 3)
            {
                stageWay = 2;
                willAttackedWhiteCheckerCountWhite++;
                mdx = true;
                enemyAttackJ = tj + 2;
                enemyAttackI = ti - 2;
                if (map[ti - 1, tj + 1] == 2) map[ti - 1, tj + 1] = -2;
                if (map[ti - 1, tj + 1] == 22) map[ti - 1, tj + 1] = -22;
                if (!AttackWaysWhite(enemyAttackI, enemyAttackJ))
                {
                    if (willAttackedWhiteCheckerCountWhite * 10 > maxWhatWhite)
                    {
                        maxWhatWhite = willAttackedWhiteCheckerCountWhite * 10;
                    }
                }
                if (map[ti - 1, tj + 1] == -2) map[ti - 1, tj + 1] = 2;
                if (map[ti - 1, tj + 1] == -22) map[ti - 1, tj + 1] = 22;
                willAttackedWhiteCheckerCountWhite--;
            }
            if (ti + 2 <= 7 && tj - 2 >= 0 && (map[ti + 1, tj - 1] == 2 || map[ti + 1, tj - 1] == 22) && (map[ti + 2, tj - 2] % 10 == 0 || (ti + 2 == globalIWhite && tj - 2 == globalJWhite
                && map[globalI, globalJ] == 2)) && stageWay != 2)
            {
                stageWay = 3;
                willAttackedWhiteCheckerCountWhite++;
                mdx = true;
                enemyAttackJ = tj - 2;
                enemyAttackI = ti + 2;
                if (map[ti + 1, tj - 1] == 2) map[ti + 1, tj - 1] = -2;
                if (map[ti + 1, tj - 1] == 22) map[ti + 1, tj - 1] = -22;
                if (!AttackWaysWhite(enemyAttackI, enemyAttackJ))
                {
                    if (willAttackedWhiteCheckerCountWhite * 10 > maxWhatWhite)
                    {
                        maxWhatWhite = willAttackedWhiteCheckerCountWhite * 10;
                    }
                }
                if (map[ti + 1, tj - 1] == -2) map[ti + 1, tj - 1] = 2;
                if (map[ti + 1, tj - 1] == -22) map[ti + 1, tj - 1] = 22;
                willAttackedWhiteCheckerCountWhite--;
            }
            if (ti + 2 <= 7 && tj + 2 <= 7 && (map[ti + 1, tj + 1] == 2 || map[ti + 1, tj + 1] == 22) && (map[ti + 2, tj + 2] % 10 == 0 || (ti + 2 == globalI && tj + 2 == globalJWhite
                && map[globalI, globalJ] == 2)) && stageWay != 1)
            {
                stageWay = 4;
                willAttackedWhiteCheckerCountWhite++;
                mdx = true;
                enemyAttackJ = tj + 2;
                enemyAttackI = ti + 2;
                if (map[ti + 1, tj + 1] == 2) map[ti + 1, tj + 1] = -2;
                if (map[ti + 1, tj + 1] == 22) map[ti + 1, tj + 1] = -22;
                if (!AttackWaysWhite(enemyAttackI, enemyAttackJ))
                {
                    if (willAttackedWhiteCheckerCountWhite * 10 > maxWhatWhite)
                    {
                        maxWhatWhite = willAttackedWhiteCheckerCountWhite * 10;
                    }
                }
                if (map[ti + 1, tj + 1] == -2) map[ti + 1, tj + 1] = 2;
                if (map[ti + 1, tj + 1] == -22) map[ti + 1, tj + 1] = 22;
                willAttackedWhiteCheckerCountWhite--;
            }
            stageWay = 0;
            return mdx;
        }
        public bool AttackWaysWhiteDam(int ti, int tj)
        {

            bool mdx = false, mdx1 = false;
            if (ti >= 0 && tj >= 0)
                for (int attackedPostI = ti - 1, attackedPostJ = tj - 1; ; attackedPostI--, attackedPostJ--)
                {
                    if (attackedPostI - 1 < 0 && attackedPostJ - 1 < 0 || attackedPostI - 1 < 0 || attackedPostJ - 1 < 0) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && (map[attackedPostI + 1, attackedPostJ + 1] <= 0
                        || map[attackedPostI + 1, attackedPostJ + 1] == 11) && map[attackedPostI - 1, attackedPostJ - 1] <= 0)
                    {
                        stageWay = 1;
                        attackCounter++;

                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 2) map[attackedPostI, attackedPostJ] = -2;
                        if (map[attackedPostI, attackedPostJ] == 22) map[attackedPostI, attackedPostJ] = -22;
                        int nextAtkI = attackedPostI - 1, nextAtkJ = attackedPostJ - 1;
                        for (; nextAtkI >= 0 && nextAtkJ >= 0; nextAtkI--, nextAtkJ--, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysWhiteDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDamWh)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDamWh = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI < 0 || nextAtkJ < 0) { nextAtkI++; nextAtkJ++; }
                            mapPrDam[mapPrCountDam] = 10 * nextAtkI + nextAtkJ;//++
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDamWh - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }

                        if (map[attackedPostI, attackedPostJ] == -2) map[attackedPostI, attackedPostJ] = 2;
                        if (map[attackedPostI, attackedPostJ] == -22) map[attackedPostI, attackedPostJ] = 22;
                        attackCounter--;

                    }
                }
            if (ti >= 0 && tj <= 7)

                for (int attackedPostI = ti - 1, attackedPostJ = tj + 1; ; attackedPostI--, attackedPostJ++)
                {
                    if (attackedPostI - 1 < 0 && attackedPostJ + 1 > 7 || attackedPostI - 1 < 0 || attackedPostJ + 1 > 7) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && map[attackedPostI - 1, attackedPostJ + 1] <= 0
                        && (map[attackedPostI + 1, attackedPostJ - 1] <= 0 || map[attackedPostI + 1, attackedPostJ - 1] == 11))
                    {

                        stageWay = 2;
                        attackCounter++;

                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 2) map[attackedPostI, attackedPostJ] = -2;
                        if (map[attackedPostI, attackedPostJ] == 22) map[attackedPostI, attackedPostJ] = -22;
                        int nextAtkI = attackedPostI - 1, nextAtkJ = attackedPostJ + 1;
                        for (; nextAtkI >= 0 && nextAtkJ <= 7; nextAtkI--, nextAtkJ++, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysWhiteDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDamWh)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDamWh = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI < 0 || nextAtkJ > 7) { nextAtkI++; nextAtkJ--; }
                            mapPrDam[mapPrCountDam] = 10 * nextAtkI + nextAtkJ;//+-
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDamWh - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }
                        if (map[attackedPostI, attackedPostJ] == -2) map[attackedPostI, attackedPostJ] = 2;
                        if (map[attackedPostI, attackedPostJ] == -22) map[attackedPostI, attackedPostJ] = 22;
                        attackCounter--;

                    }
                }
            if (ti <= 7 && tj >= 0)
                for (int attackedPostI = ti + 1, attackedPostJ = tj - 1; ; attackedPostI++, attackedPostJ--)
                {
                    if (attackedPostI + 1 > 7 && attackedPostJ - 1 < 0 || attackedPostI + 1 > 7 || attackedPostJ - 1 < 0) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && (map[attackedPostI + 1, attackedPostJ - 1] == 0
                        || map[attackedPostI + 1, attackedPostJ - 1] < 0)
                        && (map[attackedPostI - 1, attackedPostJ + 1] == 0 || map[attackedPostI - 1, attackedPostJ + 1] == 11 || map[attackedPostI - 1, attackedPostJ + 1] < 0))
                    {
                        stageWay = 3;
                        attackCounter++;

                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 2) map[attackedPostI, attackedPostJ] = -2;
                        if (map[attackedPostI, attackedPostJ] == 22) map[attackedPostI, attackedPostJ] = -22;
                        int nextAtkI = attackedPostI + 1, nextAtkJ = attackedPostJ - 1;
                        for (; nextAtkI <= 7 && nextAtkJ >= 0; nextAtkI++, nextAtkJ--, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysWhiteDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDamWh)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDamWh = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI > 7 || nextAtkJ < 0) { nextAtkI--; nextAtkJ++; }
                            mapPrDam[mapPrCountDam] = 10 * (nextAtkI) + (nextAtkJ);//-+
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDamWh - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }
                        if (map[attackedPostI, attackedPostJ] == -2) map[attackedPostI, attackedPostJ] = 2;
                        if (map[attackedPostI, attackedPostJ] == -22) map[attackedPostI, attackedPostJ] = 22;
                        attackCounter--;

                    }
                }
            if (ti <= 7 && tj <= 7)
                for (int attackedPostI = ti + 1, attackedPostJ = tj + 1; ; attackedPostI++, attackedPostJ++)
                {
                    if (attackedPostI + 1 > 7 && attackedPostJ + 1 > 7 || attackedPostI + 1 > 7 || attackedPostJ + 1 > 7) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && map[attackedPostI + 1, attackedPostJ + 1] <= 0
                        && (map[attackedPostI - 1, attackedPostJ - 1] <= 0 || map[attackedPostI - 1, attackedPostJ - 1] == 11))
                    {
                        stageWay = 4;

                        attackCounter++;

                        mdx = true;
                        if (map[attackedPostI, attackedPostJ] == 2) map[attackedPostI, attackedPostJ] = -2;
                        if (map[attackedPostI, attackedPostJ] == 22) map[attackedPostI, attackedPostJ] = -22;
                        int nextAtkI = attackedPostI + 1, nextAtkJ = attackedPostJ + 1;
                        for (; nextAtkI <= 7 && nextAtkJ <= 7; nextAtkI++, nextAtkJ++, ifm--)
                        {
                            moble[ifm] = nextAtkI * 10 + nextAtkJ;
                            ifm++;
                            if (!AttackWaysWhiteDam(nextAtkI, nextAtkJ))
                            {

                                if (attackCounter > maxWhatDamWh)
                                {
                                    for (int yr = 0; yr < 20; yr++) moble2[yr] = moble[yr];
                                    currentPosDam = attackCounter;
                                    maxWhatDamWh = attackCounter;
                                    isEndOfAttcakDam = true;
                                    mapPrCountDam = 0;
                                }
                            }
                        }
                        if (isEndOfAttcakDam && currentPosDam == attackCounter)
                        {
                            if (nextAtkI > 7 || nextAtkJ > 7) { nextAtkI--; nextAtkJ--; }
                            mapPrDam[mapPrCountDam] = 10 * (nextAtkI) + (nextAtkJ);//++
                            mapPrCountDam++;
                            currentPosDam--;
                        }
                        if (maxWhatDamWh - mapPrCountDam == 0)
                        {
                            isEndOfAttcakDam = false;
                            mapPrCountDam = 0;
                        }
                        if (map[attackedPostI, attackedPostJ] == -2) map[attackedPostI, attackedPostJ] = 2;
                        if (map[attackedPostI, attackedPostJ] == -22) map[attackedPostI, attackedPostJ] = 22;
                        attackCounter--;

                    }
                }
            return mdx;
        }
        public bool WhiteCountCheckers() {
            
            int checkersWhiteCount = 0;
            int checkersWhiteCountAttackImpossible = 0;
            bool attackpossible =false;
            for (int i=0;i<8;i++) {
                for (int j=0;j<8;j++) {
                    if (map[i, j] == 1 || map[i, j] == 11) checkersWhiteCount++;
                    if (!AttackContinues(i, j) &&( map[i, j] == 1 || map[i, j] == 11)&&( map[i-1,j-1] !=0 || map[i-1, j +1] != 0))
                        checkersWhiteCountAttackImpossible++;
                }
            }
            if ((checkersWhiteCount == 0) || checkersWhiteCountAttackImpossible == checkersWhiteCount)
            {
                System.IO.Stream resourceStream =
                    assembly.GetManifestResourceStream(@"CheckersAI.Defeat.wav");
                SoundPlayer sndpl = new SoundPlayer(resourceStream);
                sndpl.Play();
                MessageBox.Show("Черные победили");
                moveBiz = true;
                attackpossible = true;
            }
            
            return attackpossible;
        }
        public bool AttackContinuesDam(int ti, int tj) {
            bool mdx = false;
            if (ti > 0 && tj > 0)
                for (int attackedPostI = ti - 1, attackedPostJ = tj - 1; ; attackedPostI--, attackedPostJ--)
                {
                    if (attackedPostI - 1 < 0 && attackedPostJ - 1 < 0 || attackedPostI - 1 < 0 || attackedPostJ - 1 < 0) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && map[attackedPostI - 1, attackedPostJ - 1] == 0
                        && (map[attackedPostI + 1, attackedPostJ + 1] == 0 || map[attackedPostI + 1, attackedPostJ + 1] == 11))
                    {
                        return true;
                    }
                }
            if (ti > 0 && tj < 7)
                for (int attackedPostI = ti - 1, attackedPostJ = tj + 1; ; attackedPostI--, attackedPostJ++)
                {
                    if (attackedPostI - 1 < 0 && attackedPostJ + 1 > 7 || attackedPostI - 1 < 0 || attackedPostJ + 1 > 7) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && map[attackedPostI - 1, attackedPostJ + 1] == 0
                        && (map[attackedPostI + 1, attackedPostJ - 1] == 0 || map[attackedPostI + 1, attackedPostJ - 1] == 11))
                    {
                        return true;
                    }
                }
            if (ti < 7 && tj > 0)
                for (int attackedPostI = ti + 1, attackedPostJ = tj - 1; ; attackedPostI++, attackedPostJ--)
                {
                    if (attackedPostI + 1 > 7 && attackedPostJ - 1 < 0 || attackedPostI + 1 > 7 || attackedPostJ - 1 < 0) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && map[attackedPostI + 1, attackedPostJ - 1] == 0
                        && (map[attackedPostI - 1, attackedPostJ + 1] == 0 || map[attackedPostI - 1, attackedPostJ + 1] == 11))
                    {
                        return true;
                    }
                }
            if (ti < 7 && tj < 7)
                for (int attackedPostI = ti + 1, attackedPostJ = tj + 1; ; attackedPostI++, attackedPostJ++)
                {
                    if (attackedPostI + 1 > 7 && attackedPostJ + 1 > 7 || attackedPostI + 1 > 7 || attackedPostJ + 1 > 7) break;
                    if (map[attackedPostI, attackedPostJ] == 1 || map[attackedPostI, attackedPostJ] == 11) break;
                    if ((map[attackedPostI, attackedPostJ] == 2 || map[attackedPostI, attackedPostJ] == 22) && map[attackedPostI + 1, attackedPostJ + 1] == 0
                        && (map[attackedPostI - 1, attackedPostJ - 1] == 0 || map[attackedPostI - 1, attackedPostJ - 1] == 11))
                    {
                        return true;
                    }
                }
            return mdx;
        }

        public bool AttackContinues(int ti, int tj) {
            bool mdx = false;
            if (map[ti, tj] == 1 || map[ti, tj] == 11)
            {
                if (ti - 2 >= 0 && tj - 2 >= 0)
                {
                    if ((map[ti - 1, tj - 1] == 2 || map[ti - 1, tj - 1] == 22) && map[ti - 2, tj - 2] == 0)
                    {
                        mdx = true;
                    }
                }
                if (ti - 2 >= 0 && tj + 2 <= 7)
                {
                    if ((map[ti - 1, tj + 1] == 2 || map[ti - 1, tj + 1] == 22) && map[ti - 2, tj + 2] == 0)
                    {
                        mdx = true;
                    }
                }
                if (ti + 2 <= 7 && tj - 2 >= 0)
                {
                    if ((map[ti + 1, tj - 1] == 2 || map[ti + 1, tj - 1] == 22) && map[ti + 2, tj - 2] == 0)
                    {
                        mdx = true;
                    }
                }
                if (ti + 2 <= 7 && tj + 2 <= 7)
                {
                    if ((map[ti + 1, tj + 1] == 2 || map[ti + 1, tj + 1] == 22) && map[ti + 2, tj + 2] == 0)
                    {
                        mdx = true;
                    }
                }
            }
            return mdx;
        }
        public void isAttackedNow() {
           
                map[(CordYToIndex(choosenBox.Location.Y) + CordYToIndex(prevBox.Location.Y)) / 2, (CordXToIndex(choosenBox.Location.X) + CordXToIndex(prevBox.Location.X)) / 2] = 0;
                isAttack = false;
                System.IO.Stream resourceStream =
                    assembly.GetManifestResourceStream(@"CheckersAI.game_piece_movement_09.wav");
                SoundPlayer sndpl = new SoundPlayer(resourceStream);
                sndpl.Play();
            
        }
        public bool isWayCheckDam(int ti , int tj, int chI, int chJ) {
            side = 0;
                if (ti < chI && tj < chJ && Math.Abs(chI - ti) == Math.Abs(chJ - tj) && map[ti, tj] == 0) 
                {
                    for (int i = ti, j = tj; i < chI; i++, j++) if (map[i, j] == 1 || map[i, j] == 11) return false;
                    for (int i = ti + 1, j = tj + 1; i < chI - 1; i++, j++) if (map[i, j] == 2 && map[i + 1, j + 1] == 2 || map[i, j] == 22 && map[i + 1, j + 1] == 22
                        || map[i, j] == 2 && map[i + 1, j + 1] == 22 || map[i, j] == 22 && map[i + 1, j + 1] == 2) return false;
                    side = 1;
                    return true;
                }
                else if (ti < chI && tj > chJ && Math.Abs(chI - ti) == Math.Abs(chJ - tj) && map[ti, tj] == 0)
                {
                    for (int i = ti, j = tj; i < chI; i++, j--) if (map[i, j] == 1 || map[i, j] == 11) return false;
                    for (int i = ti + 1, j = tj - 1; i < chI - 1; i++, j--) if (map[i, j] == 2 && map[i + 1, j - 1] == 2 || map[i, j] == 22 && map[i + 1, j - 1] == 22
                        || map[i, j] == 2 && map[i + 1, j - 1] == 22 || map[i, j] == 22 && map[i + 1, j - 1] == 2) return false;
                    side = 2;
                    return true;
                }
                else if (ti > chI && tj < chJ && Math.Abs(chI - ti) == Math.Abs(chJ - tj) && map[ti, tj] == 0)
                {
                    for (int i = ti, j = tj; i > chI; i--, j++) if (map[i, j] == 1 || map[i, j] == 11) return false;
                    
                    for (int i = ti - 1, j = tj + 1; i > chI + 1; i--, j++) if (map[i, j] == 2 && map[i - 1, j + 1] == 2 || map[i, j] == 22 && map[i - 1, j + 1] == 22
                        || map[i, j] == 2 && map[i - 1, j + 1] == 22 || map[i, j] == 22 && map[i - 1, j + 1] == 2) return false;
                    side = 3;
                    return true;
                }
                else if (ti > chI && tj > chJ && Math.Abs(chI - ti) == Math.Abs(chJ - tj) && map[ti, tj] == 0)
                {
                    for (int i = ti, j = tj; i > chI; i--, j--) if (map[i, j] == 1 || map[i, j] == 11) return false;
                    for (int i = ti - 1, j = tj - 1; i > chI + 1; i--, j--) if (map[i, j] == 2 && map[i - 1, j - 1] == 2 || map[i, j] == 22 && map[i - 1, j - 1] == 22
                        || map[i, j] == 2 && map[i - 1, j - 1] == 22 || map[i, j] == 22 && map[i - 1, j - 1] == 2) return false;
                    side = 4;
                    return true;
                }
            return false;
        }
        public int DeleteBlackCheckersOnDamWay() {
            int blackAttackedCheckersDam = 0;
           // MessageBox.Show(CordYToIndex(prevBox.Location.Y) + "");
            switch (side)
            {
                case 1:
                    for (int i = CordYToIndex(choosenBox.Location.Y), j = CordXToIndex(choosenBox.Location.X); i < CordYToIndex(prevBox.Location.Y); i++, j++)
                        if (map[i, j] == 2 || map[i, j] == 22)
                        {
                            blackAttackedCheckersDam++;
                            map[i, j] = 0;
                        }
                    moveBiz = false;
                    break;
                case 2:
                    for (int i = CordYToIndex(choosenBox.Location.Y), j = CordXToIndex(choosenBox.Location.X); i < CordYToIndex(prevBox.Location.Y); i++, j--)
                        if (map[i, j] == 2 || map[i, j] == 22)
                        {
                            blackAttackedCheckersDam++;
                            map[i, j] = 0;
                        }
                    moveBiz = false;
                    break;
                case 3:
                    for (int i = CordYToIndex(choosenBox.Location.Y), j = CordXToIndex(choosenBox.Location.X); i > CordYToIndex(prevBox.Location.Y); i--, j++)
                        if (map[i, j] == 2 || map[i, j] == 22)
                        {
                            blackAttackedCheckersDam++;
                            map[i, j] = 0;
                        }
                    moveBiz = false;
                    break;
                case 4:
                    for (int i = CordYToIndex(choosenBox.Location.Y), j = CordXToIndex(choosenBox.Location.X); i > CordYToIndex(prevBox.Location.Y); i--, j--)
                        if (map[i, j] == 2 || map[i, j] == 22)
                        {
                            blackAttackedCheckersDam++;
                            map[i, j] = 0;
                        }
                    moveBiz = false;
                    break;
            }
            return blackAttackedCheckersDam;
        }
        public bool isAttackCheck(PictureBox tempBox) {
            bool mode = false;
            if (map[CordYToIndex(tempBox.Location.Y), CordXToIndex(tempBox.Location.X)] == 0
                && (map[CordYToIndex(choosenBox.Location.Y), CordXToIndex(choosenBox.Location.X)] == 1)   )     
            {
                if (CordYToIndex(choosenBox.Location.Y) - 2 >= 0 && CordXToIndex(choosenBox.Location.X) - 2 >= 0)
                {
                    if ((map[CordYToIndex(choosenBox.Location.Y) - 1, CordXToIndex(choosenBox.Location.X) - 1] == 2 || map[CordYToIndex(choosenBox.Location.Y) - 1, CordXToIndex(choosenBox.Location.X) - 1] == 22)
                    && map[CordYToIndex(choosenBox.Location.Y) - 2, CordXToIndex(choosenBox.Location.X) - 2] == 0
                    && (CordYToIndex(tempBox.Location.Y) == CordYToIndex(choosenBox.Location.Y) - 2 && CordXToIndex(tempBox.Location.X) == CordXToIndex(choosenBox.Location.X) - 2)) mode = true;
                }
                if (CordYToIndex(choosenBox.Location.Y) - 2 >= 0 && CordXToIndex(choosenBox.Location.X) + 2 <= 7)
                {
                    if ((map[CordYToIndex(choosenBox.Location.Y) - 1, CordXToIndex(choosenBox.Location.X) + 1] == 2 || map[CordYToIndex(choosenBox.Location.Y) - 1, CordXToIndex(choosenBox.Location.X) + 1] == 22)
                    && map[CordYToIndex(choosenBox.Location.Y) - 2, CordXToIndex(choosenBox.Location.X) + 2] == 0
                    && (CordYToIndex(tempBox.Location.Y) == CordYToIndex(choosenBox.Location.Y) - 2 && CordXToIndex(tempBox.Location.X) == CordXToIndex(choosenBox.Location.X) + 2)) mode = true;
                }
                if (CordYToIndex(choosenBox.Location.Y) + 2 <= 7 && CordXToIndex(choosenBox.Location.X) - 2 >= 0)
                {
                    if ((map[CordYToIndex(choosenBox.Location.Y) + 1, CordXToIndex(choosenBox.Location.X) - 1] == 2 || map[CordYToIndex(choosenBox.Location.Y) + 1, CordXToIndex(choosenBox.Location.X) - 1] == 22)
                    && map[CordYToIndex(choosenBox.Location.Y) + 2, CordXToIndex(choosenBox.Location.X) - 2] == 0
                    && (CordYToIndex(tempBox.Location.Y) == CordYToIndex(choosenBox.Location.Y) + 2 && CordXToIndex(tempBox.Location.X) == CordXToIndex(choosenBox.Location.X) - 2)) mode = true;
                }
                if (CordYToIndex(choosenBox.Location.Y) + 2 <= 7 && CordXToIndex(choosenBox.Location.X) + 2 <= 7)
                {
                    if ((map[CordYToIndex(choosenBox.Location.Y) + 1, CordXToIndex(choosenBox.Location.X) + 1] == 2 || map[CordYToIndex(choosenBox.Location.Y) + 1, CordXToIndex(choosenBox.Location.X) + 1] == 22)
                    && map[CordYToIndex(choosenBox.Location.Y) + 2, CordXToIndex(choosenBox.Location.X) + 2] == 0
                    && (CordYToIndex(tempBox.Location.Y) == CordYToIndex(choosenBox.Location.Y) + 2 && CordXToIndex(tempBox.Location.X) == CordXToIndex(choosenBox.Location.X) + 2)) mode = true;
                }
            }
            return mode;
        }
        public void RefreshField() {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    map[i, j] = RGmap[i, j];
                    PictureBox pBox = new PictureBox();
                    pBox.Size = new Size(81, 80);
                    pBox.Location = new Point(j * 81 + 68, i * 78 + 67);
                    pBox.BackColor = Color.Transparent;
                    if ((j % 2 + i % 2) == 1 && i < 3) pBox.Image = imageList1.Images[1];
                    else if (i > 4 && (j % 2 + i % 2) == 1) pBox.Image = imageList1.Images[0];
                    pBox.Click += new EventHandler(ClickOnCell);
                    pBox.MouseEnter += new EventHandler(MouseEnterInCell);
                    
                    this.Controls.Add(pBox);
                } 
            }
        }
        public void RefreshMapImage() {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PictureBox pBox = new PictureBox();
                    pBox.Size = new Size(81, 80);
                    pBox.Location = new Point(j * 81 + 68, i * 78 + 67);
                    pBox.BackColor = Color.Transparent;
                    if (map[i, j] > 0 && map[i, j] % 10 == 0) map[i, j] = 0;
                    if (map[i, j] < 0) map[i, j] = -map[i, j];
                    if (map[i, j] == 2) pBox.Image = imageList1.Images[1];
                    else if (map[i, j] == 1) pBox.Image = imageList1.Images[0];
                    else if (map[i, j] == 22) pBox.Image = imageList1.Images[3];
                    else if (map[i, j] == 11) pBox.Image = imageList1.Images[2];
                    pBox.Click += new EventHandler(ClickOnCell);
                    pBox.MouseEnter += new EventHandler(MouseEnterInCell);
                    this.Controls.Add(pBox);
                }
            }
        }

        public bool cAttackNow = false;
        public void ClickOnCell(object sender, EventArgs e)
        {
            int delBlDam = 0;
            if (prevBox != null)
                prevBox.BackColor = Color.Transparent;
            PictureBox pressedBox = sender as PictureBox;
            if (cAttackNow && pressedBox.Image != null) pressedBox = comboAttackBox;
            pressedBox.BackColor = invalidCell;
            attackCount = 0;

                choosenBox = pressedBox;
            
            System.IO.Stream resourceStream =
                    assembly.GetManifestResourceStream(@"CheckersAI.movement_01 (online-audio-converter.com).wav");
            SoundPlayer player = new SoundPlayer(resourceStream);
            if (pressedBox.Image == null && tipBoxClr == validCell && moveBiz)
            {
                cAttackNow = false;
                if (map[CordYToIndex(prevBox.Location.Y), CordXToIndex(prevBox.Location.X)] == 1)
                {
                    if (Math.Abs(CordXToIndex(pressedBox.Location.X) - CordXToIndex(prevBox.Location.X)) == 2) attackCount = 1;
                    else attackCount = 0;
                    map[CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X)] = 1;
                    map[CordYToIndex(prevBox.Location.Y), CordXToIndex(prevBox.Location.X)] = 0;
                    if (isAttack) isAttackedNow();
                    else
                    {
                        player.Play();
                    }
                    if (CordYToIndex(pressedBox.Location.Y) == 0 && !AttackWaysWhite(CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X)))
                        map[CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X)] = 11;

                    moveBiz = false;


                }
                else if (map[CordYToIndex(prevBox.Location.Y), CordXToIndex(prevBox.Location.X)] == 11) {
                    map[CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X)] = 11;
                    map[CordYToIndex(prevBox.Location.Y), CordXToIndex(prevBox.Location.X)] = 0;
                    delBlDam = DeleteBlackCheckersOnDamWay();
                    if (delBlDam > 0) {
                        System.IO.Stream resourceStream2 =
                    assembly.GetManifestResourceStream(@"CheckersAI.game_piece_movement_09.wav");
                        SoundPlayer player2 = new SoundPlayer(resourceStream2);
                        player2.Play();
                    }
                    else player.Play();

                    moveBiz = false;
                }
                
                //player.Play();
               
                pressedBox.Image = prevBox.Image;
                prevBox.Image = null;
                
                this.Controls.Clear();
                RefreshMapImage();
                if (AttackContinues(CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X)) && attackCount != 0
                    || (AttackContinuesDam(CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X))
                    && map[CordYToIndex(pressedBox.Location.Y), CordXToIndex(pressedBox.Location.X)] == 11 && delBlDam > 0)) moveBiz = true;
                else moveBiz = false;
                cAttackNow = moveBiz;
                if (cAttackNow) comboAttackBox = pressedBox;
                if (!moveBiz)
                {
                    blackCount = 0;
                    try
                    {
                        GenerateMoveEnemyAI();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        throw;
                    }
                    
                }
                tipBoxClr = invalidCell;
                
            }
            else if (pressedBox.Image == null)
            {
                System.IO.Stream resourceStream1 =
                    assembly.GetManifestResourceStream(@"CheckersAI.error.wav");
                SoundPlayer player1 = new SoundPlayer(resourceStream1);
                player1.Play();
            }
            
            prevBox = pressedBox;
                Program.f1.textBox1.Text = null;
            Program.f1.textBox1.Text += " ";
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Program.f1.textBox1.Text += map[i, j] + "  ";
                        if (j == 7)
                        {
                        Program.f1.textBox1.Text += Environment.NewLine + " ";
                        }
                    }
                }
        }
        public void MouseEnterInCell(object sender, EventArgs e) {
            if (map[CordYToIndex(choosenBox.Location.Y),CordXToIndex(choosenBox.Location.X)]== 1 || map[CordYToIndex(choosenBox.Location.Y), CordXToIndex(choosenBox.Location.X)] == 11)
            {
                PictureBox tipBox = sender as PictureBox;
                prevTipBox.BackColor = Color.Transparent;
                if (((map[CordYToIndex(tipBox.Location.Y), CordXToIndex(tipBox.Location.X)] == 0
                    && Math.Abs(CordXToIndex(choosenBox.Location.X) - CordXToIndex(tipBox.Location.X)) == 1
                    && Math.Abs(CordYToIndex(choosenBox.Location.Y) - CordYToIndex(tipBox.Location.Y)) == 1
                    && moveBiz &&
                    ((map[CordYToIndex(choosenBox.Location.Y), CordXToIndex(choosenBox.Location.X)] == 1
                    && (CordYToIndex(choosenBox.Location.Y) - CordYToIndex(tipBox.Location.Y) > 0))
                    || (map[CordYToIndex(choosenBox.Location.Y), CordXToIndex(choosenBox.Location.X)] == 2
                    && (CordYToIndex(tipBox.Location.Y) - CordYToIndex(choosenBox.Location.Y) > 0)))) || isAttackCheck(tipBox))
                    ||
                    (map[CordYToIndex(choosenBox.Location.Y),CordXToIndex(choosenBox.Location.X)]== 11
                     && isWayCheckDam(CordYToIndex(tipBox.Location.Y), CordXToIndex(tipBox.Location.X), CordYToIndex(choosenBox.Location.Y), CordXToIndex(choosenBox.Location.X)))
                    )
                {
                    tipBox.BackColor = validCell;
                    //tipBox.BackColor = Color.Transparent;
                    if(cAttackNow && choosenBox != comboAttackBox) tipBox.BackColor = invalidCell;
                }
                else tipBox.BackColor = invalidCell;
                prevTipBox = tipBox;
                tipBoxClr = tipBox.BackColor;
                if (isAttackCheck(tipBox)) isAttack = true;
            }
        }
        public static int CordXToIndex(int x) {
            return (x - 68) / 81;
        }
        public static int CordYToIndex(int y)
        {
            return (y - 67) / 78;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string trmpStr = "";
            for (int i = 0; i < 8; i++) trmpStr += map[1, i].ToString();
            MessageBox.Show(trmpStr);
        }
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show((CordYToIndex(choosenBox.Location.Y).ToString() + CordXToIndex(choosenBox.Location.X)).ToString());
        }
    }
}
