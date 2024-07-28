using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.HealthSystem
{
    public class Heart
    {
        public static readonly int HeartPiecesPerHeart = 4;
        const float FillPerHeartPiece = 0.5f;
        readonly Image image;

        public Heart(Image image)
        {
            this.image = image;
        }

        public int FilledHeartPieces
        {
            get { return CalculateFilledHeartPieces(); }
        }

        public int EmptyHeartPieces
        {
            get { return HeartPiecesPerHeart - CalculateFilledHeartPieces(); }
        }

        public void Replenish(int numberOfHeartPieces)
        {
            if (numberOfHeartPieces < 0) throw new ArgumentOutOfRangeException("numberOfHeartPieces");
            image.fillAmount += numberOfHeartPieces * FillPerHeartPiece;
        }

        public void Deplete(int numberOfHeartPieces)
        {
            if (numberOfHeartPieces < 0) throw new ArgumentOutOfRangeException("numberOfHeartPieces");
            image.fillAmount -= numberOfHeartPieces * FillPerHeartPiece;
        }

        private int CalculateFilledHeartPieces()
        {
            return (int)(image.fillAmount * HeartPiecesPerHeart);
        }
    }
}