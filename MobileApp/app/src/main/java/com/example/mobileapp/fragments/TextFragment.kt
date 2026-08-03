package com.example.mobileapp.fragments

import android.app.Activity.RESULT_OK
import android.content.Intent
import android.graphics.Typeface
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.speech.RecognizerIntent
import android.text.Spannable
import android.text.style.StyleSpan
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.lifecycleScope
import com.example.mobileapp.databinding.FragmentTextBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class TextFragment : Fragment() {
    companion object {
        var voice_code = 123;
        var photo_code = 111;
    }

    lateinit var binding: FragmentTextBinding
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentTextBinding.inflate(layoutInflater)
        return binding.root
    }

    fun addListItem() {
        with(binding) {
            var text= TREdit.text.toString()
            if (text.isEmpty()) {
                TREdit.setText("  * ")
            }
            else {
                TREdit.setText(text + "\n  * ")
            }
            TREdit.text?.let{
                TREdit.setSelection(it.length)
            }
        }
    }
    // todo
    // добавить сохранение данных

    fun photo() {
        var int = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI)
        startActivityForResult(int, photo_code)
    }
    lateinit var oldImageUri:Uri
    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        with(binding) {
            when(requestCode) {
                voice_code -> {
                    if (resultCode == RESULT_OK && data != null) {
                        var results = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS)
                        if (results!= null && results.size>0) {
                            var text = results[0]
                            TREdit.setText(TREdit.text.toString() + text);
                        }
                    }
                }
                photo_code -> {
                    if (resultCode == RESULT_OK && data != null) {
                        var resultUri = data.data
                        if (resultUri!= null) {
                            oldImageUri = resultUri
                        }
                    }
                }
            }
        }
    }
    fun voice() {
        var int = Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH).apply {
            putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM)
        }
        try {
            startActivityForResult(int, voice_code)
        } catch (ex:Exception) {

        }
    }


    fun boldClick() {
        with(binding) {
            var start = TREdit.selectionStart
            var end = TREdit.selectionEnd
            if (start == end) return
            var span = TREdit.text as Spannable
            var spans = span.getSpans(start, end, StyleSpan::class.java)
            if (spans.isNotEmpty()) {
                for (s in spans) {
                    span.removeSpan(s)
                }
            }
            else {
                span.setSpan(StyleSpan(Typeface.BOLD), start, end, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)
            }
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        with(binding) {
            listButton.setOnClickListener { addListItem() }
            boldButton.setOnClickListener { boldClick() }
            voiceButton.setOnClickListener { voice() }
            attachButton.setOnClickListener {
                photo()
            }
            TRSaveButton.setOnClickListener {
                lifecycleScope.launch {
                    withContext(Dispatchers.IO) {

                    }
                    if (isAdded) {
                        // todo change UI
                    }
                }
            }
        }
    }
}