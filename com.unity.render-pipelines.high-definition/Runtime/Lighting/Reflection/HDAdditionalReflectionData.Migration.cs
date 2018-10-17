using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public sealed partial class HDAdditionalReflectionData : IVersionable<HDAdditionalReflectionData.Version>
    {
        enum Version
        {
            First,
            HDProbeChild = 2,
            UseInfluenceVolume,
            MergeEditors,
            AddCaptureSettingsAndFrameSettings
        }

        static readonly MigrationDescription<Version, HDAdditionalReflectionData> k_Migration
            = MigrationDescription.New(
                MigrationStep.New(Version.MergeEditors, (HDAdditionalReflectionData t) =>
                {
                    t.infiniteProjection = !t.reflectionProbe.boxProjection;
                    t.reflectionProbe.boxProjection = false;
                }),
                MigrationStep.New(Version.UseInfluenceVolume, (HDAdditionalReflectionData t) =>
                {
                    t.influenceVolume.boxSize = t.reflectionProbe.size;
#pragma warning disable 618 // Type or member is obsolete
                    t.influenceVolume.sphereRadius = t.m_ObsoleteInfluenceSphereRadius;
                    t.influenceVolume.shape = t.m_ObsoleteInfluenceShape; //must be done after each size transfert
                    t.influenceVolume.boxBlendDistancePositive = t.m_ObsoleteBlendDistancePositive;
                    t.influenceVolume.boxBlendDistanceNegative = t.m_ObsoleteBlendDistanceNegative;
                    t.influenceVolume.boxBlendNormalDistancePositive = t.m_ObsoleteBlendNormalDistancePositive;
                    t.influenceVolume.boxBlendNormalDistanceNegative = t.m_ObsoleteBlendNormalDistanceNegative;
                    t.influenceVolume.boxSideFadePositive = t.m_ObsoleteBoxSideFadePositive;
                    t.influenceVolume.boxSideFadeNegative = t.m_ObsoleteBoxSideFadeNegative;
#pragma warning restore 618 // Type or member is obsolete
                    //Note: former editor parameters will be recreated as if non existent.
                    //User will lose parameters corresponding to non used mode between simplified and advanced
                }),
                MigrationStep.New(Version.MergeEditors, (HDAdditionalReflectionData t) =>
                {
                    t.infiniteProjection = !t.reflectionProbe.boxProjection;
                    t.reflectionProbe.boxProjection = false;
                }),
                MigrationStep.New(Version.AddCaptureSettingsAndFrameSettings, (HDAdditionalReflectionData t) =>
                {
                    t.captureSettings.shadowDistance = t.reflectionProbe.shadowDistance;
                    t.captureSettings.cullingMask = t.reflectionProbe.cullingMask;
#if UNITY_EDITOR //m_UseOcclusionCulling is not exposed in c# ! Need a cleaner API to migrate it at runtime too.
                    UnityEditor.SerializedObject serializedReflectionProbe = new UnityEditor.SerializedObject(t.reflectionProbe);
                    t.captureSettings.useOcclusionCulling = serializedReflectionProbe.FindProperty("m_UseOcclusionCulling").boolValue;
#endif
                    t.captureSettings.nearClipPlane = t.reflectionProbe.nearClipPlane;
                    t.captureSettings.farClipPlane = t.reflectionProbe.farClipPlane;
                })
            );

        //make the version name explicite to deal with inheritance
        [SerializeField, FormerlySerializedAs("version"), FormerlySerializedAs("m_Version")]
        int m_ReflectionProbeVersion;
        Version IVersionable<Version>.version
        {
            get
            {
                return (Version)m_ReflectionProbeVersion;
            }
            set
            {
                m_ReflectionProbeVersion = (int)value;
            }
        }

        #region Deprecated Fields
#pragma warning disable 649 //never assigned
        //data only kept for migration, to be removed in future version
        [SerializeField, FormerlySerializedAs("influenceShape"), System.Obsolete("Use influenceVolume parameters instead")]
        InfluenceShape m_ObsoleteInfluenceShape;
        [SerializeField, FormerlySerializedAs("influenceSphereRadius"), System.Obsolete("Use influenceVolume parameters instead")]
        float m_ObsoleteInfluenceSphereRadius = 3.0f;
        [SerializeField, FormerlySerializedAs("blendDistancePositive"), System.Obsolete("Use influenceVolume parameters instead")]
        Vector3 m_ObsoleteBlendDistancePositive = Vector3.zero;
        [SerializeField, FormerlySerializedAs("blendDistanceNegative"), System.Obsolete("Use influenceVolume parameters instead")]
        Vector3 m_ObsoleteBlendDistanceNegative = Vector3.zero;
        [SerializeField, FormerlySerializedAs("blendNormalDistancePositive"), System.Obsolete("Use influenceVolume parameters instead")]
        Vector3 m_ObsoleteBlendNormalDistancePositive = Vector3.zero;
        [SerializeField, FormerlySerializedAs("blendNormalDistanceNegative"), System.Obsolete("Use influenceVolume parameters instead")]
        Vector3 m_ObsoleteBlendNormalDistanceNegative = Vector3.zero;
        [SerializeField, FormerlySerializedAs("boxSideFadePositive"), System.Obsolete("Use influenceVolume parameters instead")]
        Vector3 m_ObsoleteBoxSideFadePositive = Vector3.one;
        [SerializeField, FormerlySerializedAs("boxSideFadeNegative"), System.Obsolete("Use influenceVolume parameters instead")]
        Vector3 m_ObsoleteBoxSideFadeNegative = Vector3.one;
#pragma warning restore 649 //never assigned
        #endregion
    }
}
