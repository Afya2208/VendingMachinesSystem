package com.example.mobileapp.fragments

import android.content.Context.MODE_PRIVATE
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.mobileapp.adapters.ReportAdapter
import com.example.mobileapp.databinding.FragmentReportsBinding
import com.example.mobileapp.models.Report
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.OkHttpClient
import okhttp3.Request
import java.time.LocalDate
import kotlin.compareTo

class ReportsFragment : Fragment() {

    lateinit var binding: FragmentReportsBinding
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentReportsBinding.inflate(layoutInflater)
        return binding.root
    }

    fun filterAndUpdate() {
        with(binding) {
            var reports = allReports
            reports = filterType(reports, 1)
            reports = filterDate(reports, startDate, endDate)
            reports = filterKeywords(reports, textInBox)
            // adapter.list = reports
            // adapter.notifyDataSetChanged()
        }
    }
    lateinit var adapter: ReportAdapter

    fun filterKeywords(reports: ArrayList<Report>, text: String) : ArrayList<Report> {
        var keys = text.split("\\s+".toRegex())
            .filter { it.isNotBlank() }
            .map { it.lowercase() }
        if (keys.isEmpty()) return arrayListOf()
        else {
            return ArrayList(reports.filter { report ->
                var titleLower = report.title?.lowercase()
                var textLower = report.text?.lowercase()

                keys.all { key ->
                    titleLower!!.contains(key) || textLower!!.contains(key)
                }
            } )
        }

    }

    fun filterDate(reports: ArrayList<Report>, startDate: LocalDate, endDate: LocalDate)
    : ArrayList<Report> {
        return ArrayList(reports.filter { report ->
            (startDate == null || report.dateKotlin!! >= startDate) &&
                    (endDate == null || report.dateKotlin!! <= endDate)
        } )
    }
    fun filterType(reports: ArrayList<Report>, typeId:Int) : ArrayList<Report> {
        return ArrayList(reports.filter { report ->
            report.typeId == typeId
        } )

    }

    lateinit var startDate: LocalDate
    lateinit var endDate: LocalDate
    lateinit var textInBox: String

    var allReports : ArrayList<Report> = ArrayList()

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        with(binding) {
            lifecycleScope.launch {
                withContext(Dispatchers.IO) {
                    readReports()
                    withContext(Dispatchers.Main) {
                        adapter = ReportAdapter(requireActivity(), allReports)
                        reportsView.adapter = adapter
                        reportsView.layoutManager = LinearLayoutManager(requireActivity())

                    }
                }
            }
        }
    }

    suspend fun readReports() = withContext(Dispatchers.IO) {
        try {
            var userId = 1;
            requireActivity().getSharedPreferences("settings", MODE_PRIVATE).apply {
                userId = getInt("userId", 1)
            }
            var url = "/reports/${userId}"
            var client = OkHttpClient()
            var req = Request.Builder().url(url).get().build()
            var response = client.newCall(req).execute()
            var resultType = object : TypeToken<ArrayList<Report>>() {

            }.type
            var body = response.body?.string()
            var result: ArrayList<Report> = Gson().fromJson(body, resultType)
            allReports = result
        } catch (ex: Exception) {
            withContext(Dispatchers.Main) {
                Toast.makeText(requireActivity(), ex.message, Toast.LENGTH_LONG).show()
            }
        }
    }

}