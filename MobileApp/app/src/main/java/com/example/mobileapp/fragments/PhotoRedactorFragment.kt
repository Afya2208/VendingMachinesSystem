package com.example.mobileapp.fragments

import android.graphics.Bitmap
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import com.canhub.cropper.CropImageView
import com.example.mobileapp.databinding.FragmentPhotoRedactorBinding

class PhotoRedactorFragment(var bitmap: Bitmap? = null) : Fragment() {


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

    }

    lateinit var binding: FragmentPhotoRedactorBinding
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentPhotoRedactorBinding.inflate(layoutInflater)
        return binding.root
    }



    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        with(binding) {
            cropperView.setImageBitmap(bitmap)
            cropperView.setOnCropImageCompleteListener(object : CropImageView.OnCropImageCompleteListener {
                override fun onCropImageComplete(
                    view: CropImageView,
                    result: CropImageView.CropResult
                ) {
                    Toast.makeText(requireActivity(), "Изменения получились!", Toast.LENGTH_LONG).show();
                }
            })
            PRSaveButton.setOnClickListener { cropperView.getCroppedImage()}
            PRRotateButton.setOnClickListener { cropperView.rotation += 90 }
        }
    }
}