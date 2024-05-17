using System;

namespace Rubix_Cube
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cube cube = new Cube(3, 3, 3);
            cube.AssembleCube();
            cube.ColourCube();

            string[] sexyMove = { "r", "u", "r '", "u '" };

            for (int repetition = 0; repetition <= 6; repetition++)
            {
                foreach (string turn in sexyMove)
                {
                    cube.Turn(turn, 1);
                }
            }
        }
    }
}
