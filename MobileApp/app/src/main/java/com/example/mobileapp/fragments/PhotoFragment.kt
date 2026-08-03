package com.example.mobileapp.fragments

import android.graphics.Bitmap
import android.net.Uri
import android.os.Bundle
import android.os.CountDownTimer
import android.os.Environment
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import com.example.mobileapp.databinding.FragmentPhotoBinding
import com.otaliastudios.cameraview.CameraListener
import com.otaliastudios.cameraview.CameraOptions
import com.otaliastudios.cameraview.PictureResult
import com.otaliastudios.cameraview.VideoResult
import com.otaliastudios.cameraview.controls.Facing
import com.otaliastudios.cameraview.controls.Flash
import com.otaliastudios.cameraview.controls.Mode
import java.io.File
import java.io.FileOutputStream

class PhotoFragment(var isVideo:Boolean = false) : Fragment() {

    lateinit var binding: FragmentPhotoBinding
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentPhotoBinding.inflate(layoutInflater)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        with(binding) {
            if (isVideo) {
                cameraView.mode = Mode.VIDEO
            }
        }
    }

    var timer: CountDownTimer? = null

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        with(binding) {
            cameraView.setLifecycleOwner(this@PhotoFragment)
            cameraView.addCameraListener(object : CameraListener() {
                override fun onCameraOpened(options: CameraOptions) {
                    Toast.makeText(requireActivity(), "Камера запущена!", Toast.LENGTH_LONG).show();
                    super.onCameraOpened(options)
                }
                override fun onVideoTaken(result: VideoResult) {
                    Toast.makeText(requireActivity(), "Видео сделано!", Toast.LENGTH_LONG).show();
                    super.onVideoTaken(result)
                    // сохранение видео

                    var file = File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS), "video_${System.currentTimeMillis()}.mp4")
                    FileOutputStream(file).use {
                        it.write(result.file.readBytes())
                        it.close()
                    }
                }
                override fun onPictureTaken(result: PictureResult) {
                    Toast.makeText(requireActivity(), "Фото сделано!", Toast.LENGTH_LONG).show();
                    super.onPictureTaken(result)

                    result.toBitmap({ bitmap ->
                        CF_ResultImageView.setImageBitmap(bitmap)
                        if (bitmap != null) {
                            bitmapGeneral = bitmap
                        }
                    })
                    // сохранение фото
                    var file = File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS), "photo_${System.currentTimeMillis()}.jpeg")
                    FileOutputStream(file).use {
                        it.write(result.data)
                        it.close()
                    }
                }
            })

        }
    }


    var recording = false
    fun video() {
        with(binding) {
            cameraView.mode = when(cameraView.mode) {
                Mode.PICTURE -> Mode.VIDEO
                else -> Mode.PICTURE
            }
            videoButton.text = "Режим ${cameraView.mode.name}"
        }
    }
    fun switch() {
        with(binding) {
            cameraView.facing = when(cameraView.facing) {
                Facing.BACK -> Facing.FRONT
                else -> Facing.BACK
            }
            if (cameraView.facing == Facing.BACK) {
                switchButton.text = "Задняя камера"
            }
            else {
                switchButton.text = "Передняя камера"
            }
        }
    }

    lateinit var bitmapGeneral:Bitmap
    lateinit var imageUri: Uri


    fun timerClick() {
        with(binding) {
            when(timerButton.text.toString()) {
                "Таймер 0с"-> {timerButton.text = "Таймер 3с"}
                "Таймер 3с"-> {timerButton.text = "Таймер 5с"}
                "Таймер 5с"-> {timerButton.text = "Таймер 0с"}
            }
        }
    }
    fun timer(seconds: Int) {
        if (timer!=null) timer?.cancel()
        timer = object :CountDownTimer(seconds * 1000L, 1000) {
            override fun onTick(p0: Long) {
                toast?.cancel()
                toast= Toast.makeText(requireActivity(), (p0/1000).toString(), Toast.LENGTH_SHORT)
                toast?.show()
            }

            override fun onFinish() {
                take()
            }
        }.start()
    }

    var toast: Toast? = null
    fun save() {
        if (bitmapGeneral != null) {
            (requireActivity() as MainActivity).setFragment(PhotoRedactorFragment(bitmapGeneral))
        }
        else {
            Toast.makeText(requireActivity(), "Нет изображения", Toast.LENGTH_LONG).show();
        }

    }
    fun takeClick() {
        with(binding) {
            when(timerButton.text.toString()) {
                "Таймер 3с"->{ timer(3)}
                "Таймер 5с"->{timer(5)}
                else ->{
                    take()
                }
            }
        }
    }
    fun take() {
        with(binding) {
            if (recording == true) {
                cameraView.stopVideo()
                recording = false;
                takeButton.text = "Снять"
                return
            }
            if (cameraView.mode == Mode.VIDEO) {
                var file = File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS), "video_${System.currentTimeMillis()}.mp4")
                cameraView.takeVideo(file)
                recording = true
                takeButton.text = "Закончить запись"
            }
            else {
                cameraView.takePicture()
            }
        }
    }
    fun flash() {
        with(binding) {
            cameraView.flash = when(cameraView.flash) {
                Flash.OFF -> Flash.ON
                else -> Flash.OFF
            }
            flashButton.text = "Вспышка ${cameraView.flash.name}"
        }
    }

    override fun onDestroy() {
        binding.cameraView.destroy()
        super.onDestroy()
    }
}