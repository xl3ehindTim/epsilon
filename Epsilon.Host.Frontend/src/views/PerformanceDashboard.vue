<template>
    <div v-if="data" class="performance-dashboard">
        <EnrollmentTermButtons
            :terms="data.terms"
            @on-term-selected="setTermFilter" />
        <CompetenceProfileComponent
            :data="filteredProfessionalTaskOutcomes"
            :domain="data.hboIDomain" />
        <CompetenceProfileLegend :domain="data.hboIDomain" />
        <div />
        <CompetenceGraph
            :data="filteredProfessionalTaskOutcomes"
            :domain="data.hboIDomain" />
        <PersonalDevelopmentGraph
            :data="filteredProfessionalSkillOutcomes"
            :domain="data?.hboIDomain"></PersonalDevelopmentGraph>
    </div>
    <RoundLoader v-else />
</template>

<script lang="ts" setup>
import {
    Api,
    CompetenceProfile,
    EnrollmentTerm,
    HttpResponse,
} from "../logic/Api"
import CompetenceProfileComponent from "@/components/CompetenceProfile.vue"
import CompetenceProfileLegend from "@/components/CompetenceProfileLegend.vue"
import CompetenceGraph from "@/components/CompetenceGraph.vue"
import { computed, onMounted, Ref, ref } from "vue"
import RoundLoader from "@/components/RoundLoader.vue"
import EnrollmentTermButtons from "@/components/EnrollmentTermButtons.vue"
import PersonalDevelopmentGraph from "@/components/PersonalDevelopmentGraph.vue"

const data: Ref<CompetenceProfile | undefined> = ref(undefined)
const App = new Api()

const selectedTerm = ref<EnrollmentTerm | null>(null)

const filteredProfessionalTaskOutcomes = computed(() => {
    if (!data.value?.professionalTaskOutcomes) {
        return []
    }

    if (!selectedTerm.value?.end_at) {
        return data.value?.professionalTaskOutcomes
    }

    return data.value.professionalTaskOutcomes.filter(
        (o) => o.assessedAt < selectedTerm.value?.end_at
    )
})

const filteredProfessionalSkillOutcomes = computed(() => {
    if (!data.value?.professionalSkillOutcomes) {
        return []
    }

    if (!selectedTerm.value?.end_at) {
        return data.value?.professionalSkillOutcomes
    }

    return data.value?.professionalSkillOutcomes?.filter(
        (o) => o.assessedAt < selectedTerm.value?.end_at
    )
})

onMounted(() => {
    App.component
        .componentDetail("competence_profile", {
            startDate: "02-26-2023",
            endDate: "05-26-2023",
        })
        .then((r: HttpResponse<CompetenceProfile>) => {
            data.value = r.data
        })
})

function setTermFilter(term: EnrollmentTerm): void {
    selectedTerm.value = term
}
</script>

<style scoped>
.performance-dashboard {
    grid-template-columns: 1fr;
}

@media screen and (min-width: 580px) {
    .performance-dashboard {
        display: grid;
        grid-template-columns: 1fr 5fr 1fr;
        gap: 4rem 0;
        align-items: center;
        justify-items: center;
    }
}
</style>
