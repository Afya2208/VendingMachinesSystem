package com.example.mobileapp.fragments

import android.content.Context.MODE_PRIVATE
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.mobileapp.adapters.GridViewAdapter
import com.example.mobileapp.databinding.FragmentMainBinding
import com.example.mobileapp.models.Task
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.OkHttpClient
import okhttp3.Request

class MainFragment : Fragment() {

    lateinit var binding: FragmentMainBinding
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentMainBinding.inflate(layoutInflater)
        return binding.root
    }


    var tasks : ArrayList<Task> = ArrayList()
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        with(binding) {
            lifecycleScope.launch {
                withContext(Dispatchers.IO) {
                    readTasks()
                    withContext(Dispatchers.Main) {
                        gridView.adapter =
                            GridViewAdapter(this@MainFragment.requireContext(), tasks);
                    }
                }

            }
        }
    }




    suspend fun readTasks() = withContext(Dispatchers.IO) {
        try {
            var userId = 1;
            var token: String? = ""
            requireActivity().getSharedPreferences("settings", MODE_PRIVATE).apply {
                userId = getInt("userId", 1)
                token = getString("token", "")
            }
            var url = "http://192.168.0.63:5555/tasks/${userId}"
            var client = OkHttpClient()
            var req = Request.Builder().url(url).get().header("Authorization" , "Bearer ${token}").build()
            var response = client.newCall(req).execute()
            var resultType = object : TypeToken<ArrayList<Task>>(){}.type
            var body = response.body?.string()
            tasks = Gson().fromJson(body, resultType)
        } catch (ex: Exception) {
            withContext(Dispatchers.Main) {
                Toast.makeText(requireActivity(), ex.message, Toast.LENGTH_LONG).show()
            }
        }
    }
}